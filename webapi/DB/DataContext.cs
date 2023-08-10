using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
using webapi.Controllers;

namespace webapi.DB
{
    public class DataContext
    {
        private DbSettings _dbSettings;
        private readonly ILogger<WeatherForecastController> _logger;

        public DataContext(IOptions<DbSettings> dbSettings, ILogger<WeatherForecastController> logger)
        {
            _dbSettings = dbSettings.Value;
            _logger = logger;
        }

        public IDbConnection? CreateConnection()
        {
            try
            {
                var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database}; Uid={_dbSettings.UserId}; Pwd={_dbSettings.Password};";
                return new MySqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        //Create database if it doesn't exist
        private async Task _initDatabase()
        {
            try
            {
                var connectionString = $"Server={_dbSettings.Server}; Uid={_dbSettings.UserId}; Pwd={_dbSettings.Password};";
                using var connection = new MySqlConnection(connectionString);
                var sql = $"CREATE DATABASE IF NOT EXISTS {_dbSettings.Database};";
                await connection.ExecuteAsync(sql);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        //Create tables if they don't exist
        private async Task _initTables()
        {
            using var connection = CreateConnection();
            if (connection == null)
                return;

            //await _initUsers();

            //async Task _initUsers()
            //{
            //    var sql = """
            //    IF OBJECT_ID('Users', 'U') IS NULL
            //    CREATE TABLE Users (
            //        Id INT NOT NULL PRIMARY KEY IDENTITY,
            //        Title NVARCHAR(MAX),
            //        FirstName NVARCHAR(MAX),
            //        LastName NVARCHAR(MAX),
            //        Email NVARCHAR(MAX),
            //        Role INT,
            //        PasswordHash NVARCHAR(MAX)
            //    );
            //""";
            //    await connection.ExecuteAsync(sql);
            //}
        }
    }
}

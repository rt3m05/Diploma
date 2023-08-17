using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace webapi.DB
{
    public class DataContext
    {
        private DbSettings _dbSettings;
        private readonly ILogger<DataContext> _logger;

        public DataContext(IOptions<DbSettings> dbSettings, ILogger<DataContext> logger)
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
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        //Create tables if they don't exist
        private async Task _initTables()
        {
            using var connection = CreateConnection();
            if (connection == null)
            {
                _logger.LogError("_initTables error: Connection was null.");
                return;
            }

            await _initUsers(connection);
            await _initProjects(connection);
            await _initTabs(connection);
            await _initTiles(connection);
            await _initTilesItems(connection);
        }

        private async Task _initUsers(IDbConnection connection)
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS Users (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                Nickname VARCHAR(255),
                                Email VARCHAR(255) UNIQUE NOT NULL,
                                PasswordHash VARCHAR(255) NOT NULL,
                                Image LONGBLOB
                            );";
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task _initProjects(IDbConnection connection)
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS Projects (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                UserId CHAR(36) NOT NULL,
                                Name VARCHAR(255) NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task _initTabs(IDbConnection connection)
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS Tabs (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                ProjectId CHAR(36) NOT NULL,
                                UserId CHAR(36) NOT NULL,
                                Name VARCHAR(255) NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (ProjectId) REFERENCES Projects (Id),
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task _initTiles(IDbConnection connection)
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS Tiles (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                TabId CHAR(36) NOT NULL,
                                UserId CHAR(36) NOT NULL,
                                Name VARCHAR(255) NOT NULL,
                                X SMALLINT UNSIGNED NOT NULL,
                                Y SMALLINT UNSIGNED NOT NULL,
                                H SMALLINT UNSIGNED NOT NULL,
                                W SMALLINT UNSIGNED NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (TabId) REFERENCES Tabs (Id),
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task _initTilesItems(IDbConnection connection)
        {
            try
            {
                var sql = @"CREATE TABLE IF NOT EXISTS TilesItems (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                TileId CHAR(36) NOT NULL,
                                UserId CHAR(36) NOT NULL,
                                Content VARCHAR(255) NOT NULL,
                                Type VARCHAR(255) NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (TileId) REFERENCES Tiles (Id),
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}

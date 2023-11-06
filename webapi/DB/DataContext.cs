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
        private bool _isInit;

        public DataContext(IOptions<DbSettings> dbSettings, ILogger<DataContext> logger)
        {
            _dbSettings = dbSettings.Value;
            _logger = logger;
            _isInit = false;
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
            await _initStoredProcedures();

            if (_isInit)
                _logger.LogInformation("Database is initialized.");
            else
            {
                _logger.LogError("Database is NOT initialized.");
                Environment.Exit(70);
            }
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

                _isInit = true;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                _isInit = false;
            }
        }

        //Create tables if they don't exist
        private async Task _initTables()
        {
            using var connection = CreateConnection();
            if (connection == null)
            {
                _logger.LogError("_initTables error: Connection was null.");
                _isInit = false;
                return;
            }

            try
            {
                await _initUsers(connection);
                await _initProjects(connection);
                await _initTabs(connection);
                await _initTiles(connection);
                await _initTilesItems(connection);

                _isInit = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                _isInit = false;
            }
        }

        //Create stored procedures if they don`t exist
        private async Task _initStoredProcedures()
        {
            using var connection = CreateConnection();
            if (connection == null)
            {
                _logger.LogError("_initStoredProcedures error: Connection was null.");
                _isInit = false;
                return;
            }

            try
            {
                await _initCreateTileWithPositionChange(connection);
                await _initChangeTilePosition(connection);
                await _initUpdateTileWithPositionChange(connection);
                await _initDeleteTileWithPositionChange(connection);

                await _initCreateTileItemWithPositionChange(connection);
                await _initChangeTileItemPosition(connection);
                await _initUpdateTileItemWithPositionChange(connection);
                await _initDeleteTileItemWithPositionChange(connection);

                _isInit = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                _isInit = false;
            }
        }

        private async Task _initUsers(IDbConnection connection)
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Users (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                Nickname VARCHAR(255),
                                Email VARCHAR(255) UNIQUE NOT NULL,
                                PasswordHash VARCHAR(255) NOT NULL,
                                ImageName VARCHAR(255)
                            );";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initProjects(IDbConnection connection)
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

        private async Task _initTabs(IDbConnection connection)
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

        private async Task _initTiles(IDbConnection connection)
        {
            var sql = @"CREATE TABLE IF NOT EXISTS Tiles (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                TabId CHAR(36) NOT NULL,
                                UserId CHAR(36) NOT NULL,
                                Name VARCHAR(255) NOT NULL,
                                Position TINYINT UNSIGNED NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (TabId) REFERENCES Tabs (Id),
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initTilesItems(IDbConnection connection)
        {
            var sql = @"CREATE TABLE IF NOT EXISTS TilesItems (
                                Id CHAR(36) NOT NULL PRIMARY KEY,
                                TileId CHAR(36) NOT NULL,
                                UserId CHAR(36) NOT NULL,
                                Content VARCHAR(255) NOT NULL,
                                Type VARCHAR(255) NOT NULL,
                                Position TINYINT UNSIGNED NOT NULL,
                                IsDone BOOLEAN NOT NULL,
                                TimeStamp TIMESTAMP NOT NULL,
                                FOREIGN KEY (TileId) REFERENCES Tiles (Id),
                                FOREIGN KEY (UserId) REFERENCES Users (Id)
                            );";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initCreateTileWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS CreateTileWithPositionChange(IN Id CHAR(36), IN TabId CHAR(36), IN UserId CHAR(36), IN `Name` VARCHAR(255), IN Position TINYINT UNSIGNED, IN `TimeStamp` TimeStamp)
                        BEGIN
	                        DECLARE cnt INT;

                            SELECT COUNT(Tiles.Id) INTO cnt FROM Tiles WHERE Tiles.Position = Position AND Tiles.TabId = TabId;

                            IF cnt > 0 THEN
		                        UPDATE Tiles
		                        SET Tiles.Position = Tiles.Position + 1
		                        WHERE Tiles.Position >= Position AND Tiles.TabId = TabId;
                            END IF;

                            INSERT INTO Tiles (Tiles.Id, Tiles.TabId, Tiles.UserId, Tiles.`Name`, Tiles.Position, Tiles.`TimeStamp`) VALUES (Id, TabId, UserId, `Name`, Position, `TimeStamp`);
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initChangeTilePosition(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS ChangeTilePosition(IN id CHAR(36), IN tabId CHAR(36), IN newPos TINYINT UNSIGNED, IN oldPos TINYINT UNSIGNED)
                        BEGIN
                            DECLARE cnt INT;
    
                            SELECT COUNT(id) INTO cnt FROM Tiles WHERE Position = newPos AND Tiles.TabId = tabId;

                            IF cnt > 0 THEN
                                IF newPos < oldPos THEN
			                        UPDATE Tiles
			                        SET Tiles.Position = Tiles.Position + 1
			                        WHERE Tiles.Position >= newPos AND Tiles.Position < oldPos AND Tiles.TabId = tabId;
		                        ELSE
			                        UPDATE Tiles
			                        SET Tiles.Position = Tiles.Position - 1
			                        WHERE Tiles.Position > oldPos AND Tiles.Position <= newPos AND Tiles.TabId = tabId;
		                        END IF;
                            END IF;

                            UPDATE tiles SET position = newPos WHERE tiles.id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initUpdateTileWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS UpdateTileWithPositionChange(IN Id CHAR(36), IN TabId CHAR(36), IN UserId CHAR(36), IN `Name` VARCHAR(255), IN `TimeStamp` TimeStamp, IN newPos TINYINT UNSIGNED, IN oldPos TINYINT UNSIGNED)
                        BEGIN
	                        DECLARE cnt INT;

	                        UPDATE Tiles 
	                        SET Tiles.TabId = TabId,
		                        Tiles.UserId = UserId,
		                        Tiles.`Name` = `Name`,
		                        Tiles.`TimeStamp` = `TimeStamp`
	                        WHERE Tiles.Id = Id;

                            SELECT COUNT(Tiles.Id) INTO cnt FROM Tiles WHERE Tiles.Position = newPos AND Tiles.TabId = TabId;

                            IF cnt > 0 THEN
		                        IF newPos < oldPos THEN
			                        UPDATE Tiles
			                        SET Tiles.Position = Tiles.Position + 1
			                        WHERE Tiles.Position >= newPos AND Tiles.Position < oldPos AND Tiles.TabId = TabId;
		                        ELSE
			                        UPDATE Tiles
			                        SET Tiles.Position = Tiles.Position - 1
			                        WHERE Tiles.Position > oldPos AND Tiles.Position <= newPos AND Tiles.TabId = TabId;
		                        END IF;
                            END IF;

                            UPDATE Tiles SET Tiles.Position = newPos WHERE Tiles.Id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initDeleteTileWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS DeleteTileWithPositionChange(IN id CHAR(36), IN tabId CHAR(36), IN pos TINYINT UNSIGNED)
                        BEGIN
	                        DECLARE cnt INT;

                            SELECT COUNT(Tiles.Id) INTO cnt FROM Tiles WHERE Tiles.Position > pos AND Tiles.TabId = tabId;

                            IF cnt > 0 THEN
		                        UPDATE Tiles
		                        SET Tiles.Position = Tiles.Position - 1
		                        WHERE Tiles.Position > pos AND Tiles.TabId = tabId;
                            END IF;

                            DELETE FROM Tiles WHERE Tiles.Id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initCreateTileItemWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS CreateTileItemWithPositionChange(IN Id CHAR(36), IN TileId CHAR(36), IN UserId CHAR(36), IN Content VARCHAR(255), IN `Type` VARCHAR(255), IN Position TINYINT UNSIGNED, IN IsDone BOOL, IN `TimeStamp` TimeStamp)
                        BEGIN
                            DECLARE cnt INT;

                            SELECT COUNT(TilesItems.Id) INTO cnt FROM TilesItems WHERE TilesItems.Position = Position AND TilesItems.TileId = TileId;

                            IF cnt > 0 THEN
                                UPDATE TilesItems
		                        SET TilesItems.Position = TilesItems.Position + 1
		                        WHERE TilesItems.Position >= Position AND TilesItems.TileId = TileId;
                            END IF;

                            INSERT INTO TilesItems (Id, TileId, UserId, Content, `Type`, Position, IsDone, `TimeStamp`) VALUES (Id, TileId, UserId, Content, `Type`, Position, IsDone, `TimeStamp`);
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initChangeTileItemPosition(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS ChangeTileItemPosition(IN id CHAR(36), IN tileId CHAR(36), IN newPos TINYINT UNSIGNED, IN oldPos TINYINT UNSIGNED)
                        BEGIN
                            DECLARE cnt INT;
    
                            SELECT COUNT(TilesItems.Id) INTO cnt FROM TilesItems WHERE TilesItems.Position = newPos AND TilesItems.TileId = tileId;

                            IF cnt > 0 THEN
                                IF newPos < oldPos THEN
			                        UPDATE TilesItems
			                        SET TilesItems.Position = TilesItems.Position + 1
			                        WHERE TilesItems.Position >= newPos AND TilesItems.Position < oldPos AND TilesItems.TileId = tileId;
		                        ELSE
			                        UPDATE TilesItems
			                        SET TilesItems.Position = TilesItems.Position - 1
			                        WHERE TilesItems.Position > oldPos AND TilesItems.Position <= newPos AND TilesItems.TileId = tileId;
		                        END IF;
                            END IF;

                            UPDATE TilesItems SET TilesItems.Position = newPos WHERE TilesItems.Id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initUpdateTileItemWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS UpdateTileItemWithPositionChange(IN Id CHAR(36), IN TileId CHAR(36), IN UserId CHAR(36), IN Content VARCHAR(255), IN `Type` VARCHAR(255), IN IsDone BOOL, IN `TimeStamp` TimeStamp, IN newPos TINYINT UNSIGNED, IN oldPos TINYINT UNSIGNED)
                        BEGIN
	                        DECLARE cnt INT;

	                        UPDATE TilesItems 
	                        SET TilesItems.TileId = TileId,
		                        TilesItems.UserId = UserId,
                                TilesItems.Content = Content,
                                TilesItems.`Type` = `Type`,
		                        TilesItems.IsDone = IsDone,
		                        TilesItems.`TimeStamp` = `TimeStamp`
	                        WHERE TilesItems.Id = Id;

                            SELECT COUNT(TilesItems.Id) INTO cnt FROM TilesItems WHERE TilesItems.Position = newPos AND TilesItems.TileId = TileId;

                            IF cnt > 0 THEN
                                IF newPos < oldPos THEN
			                        UPDATE TilesItems
			                        SET TilesItems.Position = TilesItems.Position + 1
			                        WHERE TilesItems.Position >= newPos AND TilesItems.Position < oldPos AND TilesItems.TileId = TileId;
		                        ELSE
			                        UPDATE TilesItems
			                        SET TilesItems.Position = TilesItems.Position - 1
			                        WHERE TilesItems.Position > oldPos AND TilesItems.Position <= newPos AND TilesItems.TileId = TileId;
		                        END IF;
                            END IF;

                            UPDATE TilesItems SET TilesItems.Position = newPos WHERE TilesItems.Id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initDeleteTileItemWithPositionChange(IDbConnection connection)
        {
            var sql = @"
                        CREATE PROCEDURE IF NOT EXISTS DeleteTileItemWithPositionChange(IN id CHAR(36), IN tileId CHAR(36), IN pos TINYINT UNSIGNED)
                        BEGIN
                            DECLARE cnt INT;

                            SELECT COUNT(TilesItems.Id) INTO cnt FROM TilesItems WHERE TilesItems.Position > pos AND TilesItems.TileId = tileId;

                            IF cnt > 0 THEN
                                UPDATE TilesItems
		                        SET TilesItems.Position = TilesItems.Position - 1
		                        WHERE TilesItems.Position > pos AND TilesItems.TileId = tileId;
                            END IF;

                            DELETE FROM TilesItems WHERE TilesItems.Id = id;
                        END;
                        ";
            await connection.ExecuteAsync(sql);
        }
    }
}

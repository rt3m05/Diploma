using Dapper;
using webapi.Models;

namespace webapi.DB.Repositories
{
    public interface ITileRepository
    {
        Task<IEnumerable<Tile>> GetAll();
        Task<IEnumerable<Tile>> GetAllByTab(Guid tabId);
        Task<Tile> GetById(Guid id);
        Task Create(Tile tile);
        Task CreateWithPositionChange(Tile tile);
        Task Update(Tile tile);
        Task UpdateWithPositionChange(Tile tile, byte newPos);
        Task ChangePosition(Guid id, Guid tabId, byte newPos, byte oldPos);
        Task Delete(Guid id);
        Task DeleteWithPositionChange(Guid id, Guid tabId, byte pos);
        Task DeleteAllByUser(Guid userId);
    }

    public class TileRepository : ITileRepository
    {
        private readonly DataContext _context;

        public TileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tile>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Tiles";
            return await connection.QueryAsync<Tile>(sql);
        }

        public async Task<IEnumerable<Tile>> GetAllByTab(Guid tabId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Tiles 
                        WHERE TabId = @tabId
                      ";
            return await connection.QueryAsync<Tile>(sql, new { tabId });
        }

        public async Task<Tile> GetById(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Tiles 
                        WHERE Id = @id
                      ";
            return await connection.QuerySingleOrDefaultAsync<Tile>(sql, new { id });
        }

        public async Task Create(Tile tile)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        INSERT INTO Tiles (Id, TabId, UserId, Name, Position, TimeStamp)
                        VALUES (@Id, @TabId, @UserId, @Name, @Position, @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, tile);
        }

        public async Task CreateWithPositionChange(Tile tile)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("CreateTileWithPositionChange", new { tile.Id, tile.TabId, tile.UserId, tile.Name, tile.Position, tile.TimeStamp }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Update(Tile tile)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE Tiles 
                        SET TabId = @TabId,
                            UserId = @UserId,
                            Name = @Name,
                            Position = @Position,
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, tile);
        }

        public async Task UpdateWithPositionChange(Tile tile, byte newPos)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UpdateTileWithPositionChange", new{ tile.Id, tile.TabId, tile.UserId, tile.Name, tile.TimeStamp, newPos, oldPos = tile.Position }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task ChangePosition(Guid id, Guid tabId, byte newPos, byte oldPos)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("ChangeTilePosition", new { id, tabId, newPos, oldPos }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Tiles 
                        WHERE Id = @id
                      ";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteWithPositionChange(Guid id, Guid tabId, byte pos)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteTileWithPositionChange", new { id, tabId, pos }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Tiles 
                        WHERE UserId = @userId
                      ";
            await connection.ExecuteAsync(sql, new { userId });
        }
    }
}
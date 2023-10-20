using Dapper;
using webapi.Models;

namespace webapi.DB.Repositories
{
    public interface ITileItemRepository
    {
        Task<IEnumerable<TileItem>> GetAll();
        Task<IEnumerable<TileItem>> GetAllByTile(Guid tileId);
        Task<TileItem> GetById(Guid id);
        Task Create(TileItem tileItem);
        Task CreateWithPositionChange(TileItem tileItem);
        Task Update(TileItem tileItem);
        Task UpdateWithPositionChange(TileItem tileItem, byte newPos);
        Task ChangePosition(Guid id, Guid tileId, byte newPos, byte oldPos);
        Task Delete(Guid id);
        Task DeleteWithPositionChange(Guid id, Guid tileId, byte pos);
        Task DeleteAllByUser(Guid userId);
    }

    public class TileItemRepository : ITileItemRepository
    {
        private readonly DataContext _context;

        public TileItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TileItem>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM TilesItems";
            return await connection.QueryAsync<TileItem>(sql);
        }

        public async Task<IEnumerable<TileItem>> GetAllByTile(Guid tileId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM TilesItems 
                        WHERE TileId = @tileId
                      ";
            return await connection.QueryAsync<TileItem>(sql, new { tileId });
        }

        public async Task<TileItem> GetById(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM TilesItems 
                        WHERE Id = @id
                      ";
            return await connection.QuerySingleOrDefaultAsync<TileItem>(sql, new { id });
        }

        public async Task Create(TileItem tileItem)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        INSERT INTO TilesItems (Id, TileId, UserId, Content, Type, Position, IsDone, TimeStamp)
                        VALUES (@Id, @TileId, @UserId, @Content, '" + tileItem.Type.ToString() + @"', @Position, @IsDone, @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, tileItem);
        }

        public async Task CreateWithPositionChange(TileItem tileItem)
        {
            using var connection = _context.CreateConnection();
            var param = new
            {
                tileItem.Id,
                tileItem.TileId,
                tileItem.UserId,
                tileItem.Content,
                tileItem.Type,
                tileItem.Position,
                tileItem.IsDone,
                tileItem.TimeStamp
            };
            await connection.ExecuteAsync("CreateTileItemWithPositionChange", param, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Update(TileItem tileItem)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE TilesItems 
                        SET TileId = @TileId,
                            UserId = @UserId,
                            Content = @Content,
                            Type = @Type,
                            Position = @Position,
                            IsDone = @IsDone,
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, tileItem);
        }

        public async Task UpdateWithPositionChange(TileItem tileItem, byte newPos)
        {
            using var connection = _context.CreateConnection();
            var param = new
            {
                tileItem.Id,
                tileItem.TileId,
                tileItem.UserId,
                tileItem.Content,
                tileItem.Type,
                tileItem.IsDone,
                tileItem.TimeStamp,
                newPos,
                oldPos = tileItem.Position
            };
            await connection.ExecuteAsync("UpdateTileItemWithPositionChange", param, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task ChangePosition(Guid id, Guid tileId, byte newPos, byte oldPos)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("ChangeTileItemPosition", new { id, tileId, newPos, oldPos }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM TilesItems 
                        WHERE Id = @id
                      ";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteWithPositionChange(Guid id, Guid tileId, byte pos)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteTileItemWithPositionChange", new { id, tileId, pos }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM TilesItems 
                        WHERE UserId = @userId
                      ";
            await connection.ExecuteAsync(sql, new { userId });
        }
    }
}

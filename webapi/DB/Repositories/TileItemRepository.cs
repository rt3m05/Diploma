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
        Task Update(TileItem tileItem);
        Task Delete(Guid id);
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
                        INSERT INTO TilesItems (Id, TileId, UserId, Content, Type, TimeStamp)
                        VALUES (@Id, @TileId, @UserId, @Content, '" + tileItem.Type.ToString() + @"', @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, tileItem);
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
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, tileItem);
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

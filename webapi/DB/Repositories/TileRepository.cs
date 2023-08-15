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
        Task Update(Tile tile);
        Task Delete(Guid id);
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
                        INSERT INTO Tiles (Id, TabId, Name, X, Y, H, W, TimeStamp)
                        VALUES (@Id, @TabId, @Name, @X, @Y, @H, @W, @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, tile);
        }

        public async Task Update(Tile tile)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE Tiles 
                        SET TabId = @TabId,
                            Name = @Name,
                            X = @X,
                            Y = @Y,
                            H = @H,
                            W = @W,
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, tile);
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
    }
}
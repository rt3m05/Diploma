using Dapper;
using webapi.Models;

namespace webapi.DB.Repositories
{
    public interface ITabRepository
    {
        Task<IEnumerable<Tab>> GetAll();
        Task<IEnumerable<Tab>> GetAllByProject(Guid projectId);
        Task<Tab> GetById(Guid id);
        Task Create(Tab tab);
        Task Update(Tab tab);
        Task Delete(Guid id);
        Task DeleteAllByUser(Guid userId);
    }

    public class TabRepository : ITabRepository
    {
        private readonly DataContext _context;

        public TabRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tab>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Tabs";
            return await connection.QueryAsync<Tab>(sql);
        }

        public async Task<IEnumerable<Tab>> GetAllByProject(Guid projectId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Tabs 
                        WHERE ProjectId = @projectId
                      ";
            return await connection.QueryAsync<Tab>(sql, new { projectId });
        }

        public async Task<Tab> GetById(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Tabs 
                        WHERE Id = @id
                      ";
            return await connection.QuerySingleOrDefaultAsync<Tab>(sql, new { id });
        }

        public async Task Create(Tab tab)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        INSERT INTO Tabs (Id, ProjectId, UserId, Name, TimeStamp)
                        VALUES (@Id, @ProjectId, @UserId, @Name, @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, tab);
        }

        public async Task Update(Tab tab)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE Tabs 
                        SET ProjectId = @ProjectId,
                            UserId = @UserId,
                            Name = @Name,
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, tab);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Tabs 
                        WHERE Id = @id
                      ";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Tabs 
                        WHERE UserId = @userId
                      ";
            await connection.ExecuteAsync(sql, new { userId });
        }
    }
}

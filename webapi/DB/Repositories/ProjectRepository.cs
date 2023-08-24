using Dapper;
using webapi.Models;

namespace webapi.DB.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAll();
        Task<IEnumerable<Project>> GetAllByUser(Guid userId);
        Task<Project> GetById(Guid id);
        Task Create(Project project);
        Task Update(Project project);
        Task Delete(Guid id);
        Task DeleteAllByUser(Guid userId);
    }

    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;

        public ProjectRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Projects";
            return await connection.QueryAsync<Project>(sql);
        }

        public async Task<IEnumerable<Project>> GetAllByUser(Guid userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Projects 
                        WHERE UserId = @userId
                      ";
            return await connection.QueryAsync<Project>(sql, new { userId });
        }

        public async Task<Project> GetById(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Projects 
                        WHERE Id = @id
                      ";
            return await connection.QuerySingleOrDefaultAsync<Project>(sql, new { id });
        }

        public async Task Create(Project project)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        INSERT INTO Projects (Id, UserId, Name, TimeStamp)
                        VALUES (@Id, @UserId, @Name, @TimeStamp)
                      ";
            await connection.ExecuteAsync(sql, project);
        }

        public async Task Update(Project project)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE Projects 
                        SET UserId = @UserId,
                            Name = @Name,
                            TimeStamp = @TimeStamp
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, project);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Projects 
                        WHERE Id = @id
                      ";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Projects 
                        WHERE UserId = @userId
                      ";
            await connection.ExecuteAsync(sql, new { userId });
        }
    }
}

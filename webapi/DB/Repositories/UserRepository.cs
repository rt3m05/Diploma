using Dapper;
using webapi.Models;

namespace webapi.DB.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> GetByEmail(string email);
        Task Create(User user);
        Task Update(User user);
        Task Delete(Guid id);
        Task Delete(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<User> GetById(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Users 
                        WHERE Id = @id
                      ";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        }

        public async Task<User> GetByEmail(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        SELECT * FROM Users 
                        WHERE Email = @email
                      ";
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        }

        public async Task Create(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        INSERT INTO Users (Id, Nickname, Email, PasswordHash, ImageName)
                        VALUES (@Id, @Nickname, @Email, @PasswordHash, @ImageName)
                      ";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task Update(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        UPDATE Users 
                        SET Nickname = @Nickname,
                            Email = @Email,
                            PasswordHash = @PasswordHash, 
                            ImageName = @ImageName
                        WHERE Id = @Id
                      ";
            await connection.ExecuteAsync(sql, user);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Users 
                        WHERE Id = @id
                      ";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task Delete(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = @"
                        DELETE FROM Users 
                        WHERE Email = @email
                      ";
            await connection.ExecuteAsync(sql, new { email });
        }
    }
}

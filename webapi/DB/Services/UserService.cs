using webapi.DB.Repositories;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.User;

namespace webapi.DB.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<Guid>  Create(CreateRequest model);
        Task Update(Guid id, UpdateRequest model);
        Task Delete(Guid id);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<Guid> Create(CreateRequest model)
        {
            if (await _userRepository.GetByEmail(model.Email!) != null)
                throw new EmailExistsException("User with the email '" + model.Email + "' already exists");

            User user = new(model);

            await _userRepository.Create(user);

            return user.Id;
        }

        public async Task Update(Guid id, UpdateRequest model)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
                throw new EmailExistsException("User with the email '" + model.Email + "' already exists");

            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            user.Nickname = model.Nickname;
            user.Email = model.Email;
            user.Image = model.Image;

            await _userRepository.Update(user);
        }

        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id);
        }
    }
}
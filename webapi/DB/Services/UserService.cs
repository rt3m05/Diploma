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
        Task<User> GetByEmail(string email);
        Task<Guid> Create(UserCreateRequest model);
        Task Update(string email, UserUpdateRequest model);
        Task Delete(Guid id);
        Task Delete(string email);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

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

        public async Task<User> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task<Guid> Create(UserCreateRequest model)
        {
            if (await _userRepository.GetByEmail(model.Email!) != null)
                throw new EmailExistsException("User with the email '" + model.Email + "' already exists");

            User user = new(model);

            await _userRepository.Create(user);

            return user.Id;
        }

        public async Task Update(string email, UserUpdateRequest model)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
            if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
                throw new EmailExistsException("User with the email '" + model.Email + "' already exists");

            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            user.Nickname = model.Nickname;
            if(!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;
            user.Image = model.Image;

            await _userRepository.Update(user);
        }

        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id);
        }

        //TODO: Add delete all info(projects, tabs ...)
        public async Task Delete(string email)
        {
            if (await _userRepository.GetByEmail(email) != null)
                await _userRepository.Delete(email);
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
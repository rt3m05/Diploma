using Microsoft.Extensions.Options;
using webapi.DB.Repositories;
using webapi.DB.Services.Settings;
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
        Task<User> GetByIdWithImage(Guid id);
        Task<User> GetByEmailWithImage(string email);
        Task<Guid> Create(UserCreateRequest model);
        Task Update(string email, UserUpdateRequest model);
        Task Delete(Guid id);
        Task Delete(string email);
    }

    public class UserService : IUserService
    {
        private UserServiceSettings _settings;
        private readonly IUserRepository _userRepository;
        private readonly IProjectService _projectService;

        public UserService(IOptions<UserServiceSettings> settings, IUserRepository userRepository, IProjectService projectService)
        {
            _settings = settings.Value;
            _userRepository = userRepository;
            _projectService = projectService;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var all = await _userRepository.GetAll();
            foreach (var item in all)
            {
                item.Image = File.ReadAllBytes(_settings.dir + "/" + item.Id.ToString() + "/" + item.ImageName);
            }
            return all;
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

        public async Task<User> GetByIdWithImage(Guid id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            if(File.Exists(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                user.Image = File.ReadAllBytes(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName);
            else
                user.Image = null;

            return user;
        }

        public async Task<User> GetByEmailWithImage(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            if(File.Exists(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                user.Image = File.ReadAllBytes(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName);
            else
                user.Image = null;

            return user;
        }

        public async Task<Guid> Create(UserCreateRequest model)
        {
            if (await _userRepository.GetByEmail(model.Email!) != null)
                throw new EmailExistsException("User with the email '" + model.Email + "' already exists");

            User user = new(model);

            if (model.Image != null && model.Image.Length > 0)
            {
                var strs = model.Image.FileName.Split('.');
                user.ImageName = Guid.NewGuid().ToString() + "." + strs.Last();

                if (!Directory.Exists(_settings.dir + "/" + user.Id.ToString()))
                {
                    Directory.CreateDirectory(_settings.dir + "/" + user.Id.ToString());
                }
                if (!File.Exists(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                {
                    using (var stream = File.Create(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                    {
                        await model.Image.CopyToAsync(stream);
                        stream.Flush();
                    }
                }
            }

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

            if (string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Nickname) && model.Image == null)
                throw new EmptyModelException("Model was empty");

            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            if(!string.IsNullOrEmpty(model.Nickname))
                user.Nickname = model.Nickname;

            if(!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;

            if (model.Image != null && model.Image.Length > 0)
            {
                if (user.ImageName != null && File.Exists(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                {
                    File.Delete(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName);
                }

                var strs = model.Image.FileName.Split('.');
                user.ImageName = Guid.NewGuid().ToString() + "." + strs.Last();

                if (!Directory.Exists(_settings.dir + "/" + user.Id.ToString()))
                {
                    Directory.CreateDirectory(_settings.dir + "/" + user.Id.ToString());
                }
                if (!File.Exists(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                {
                    using (var stream = File.Create(_settings.dir + "/" + user.Id.ToString() + "/" + user.ImageName))
                    {
                        await model.Image.CopyToAsync(stream);
                        stream.Flush();
                    }
                }
            }

            await _userRepository.Update(user);
        }

        public async Task Delete(Guid id)
        {
            var user = await _userRepository.GetById(id);
            if (user != null)
            {
                await _projectService.DeleteAllByUser(user.Id);

                if (Directory.Exists(_settings.dir + "/" + user.Id.ToString()))
                    Directory.Delete(_settings.dir + "/" + user.Id.ToString(), true);

                await _userRepository.Delete(user.Id);
            }
            else
                throw new KeyNotFoundException("User not found");
        }

        public async Task Delete(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user != null)
            {
                await _projectService.DeleteAllByUser(user.Id);

                if (Directory.Exists(_settings.dir + "/" + user.Id.ToString()))
                    Directory.Delete(_settings.dir + "/" + user.Id.ToString(), true);

                await _userRepository.Delete(user.Email!);
            }
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
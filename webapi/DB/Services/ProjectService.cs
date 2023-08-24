using webapi.DB.Repositories;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.Project;

namespace webapi.DB.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAll();
        Task<IEnumerable<Project>> GetAllByUser(string email);
        Task<Project> GetById(Guid id);
        Task<Guid> Create(ProjectCreateRequest model, string userEmail);
        Task Update(Guid id, ProjectUpdateRequest model);
        Task Delete(Guid id);
        Task DeleteAllByUser(Guid userId);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITabService _tabService;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, ITabService tabService)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _tabService = tabService;
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public async Task<IEnumerable<Project>> GetAllByUser(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return await _projectRepository.GetAllByUser(user.Id);
        }

        public async Task<Project> GetById(Guid id)
        {
            var project = await _projectRepository.GetById(id);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            return project;
        }

        public async Task<Guid> Create(ProjectCreateRequest model, string userEmail)
        {
            var user = await _userRepository.GetByEmail(userEmail);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            Project project = new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = model.Name,
                TimeStamp = DateTime.Now
            };

            await _projectRepository.Create(project);

            return project.Id;
        }

        public async Task Update(Guid id, ProjectUpdateRequest model)
        {
            if(model.Name == null)
                throw new EmptyModelException("Model was empty");

            var project = await _projectRepository.GetById(id);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            if(model.Name != null)
                project.Name = model.Name; 

            await _projectRepository.Update(project);
        }

        public async Task Delete(Guid id)
        {
            if (await _projectRepository.GetById(id) != null)
            {
                foreach(var tab in await _tabService.GetAllByProject(id))
                {
                    await _tabService.Delete(tab.Id);
                }

                await _projectRepository.Delete(id);
            }
            else
                throw new KeyNotFoundException("Project not found");
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            if (await _userRepository.GetById(userId) != null)
            {
                await _tabService.DeleteAllByUser(userId);
                await _projectRepository.DeleteAllByUser(userId);
            }
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
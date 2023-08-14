using webapi.DB.Repositories;
using webapi.Models;
using webapi.Requests.Project;

namespace webapi.DB.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAll();
        Task<IEnumerable<Project>> GetAllByUser(string email);
        Task<Project> GetById(Guid id);
        Task<Guid> Create(ProjectCreateRequest model);
        Task Update(Guid id, ProjectUpdateRequest model);
        Task Delete(Guid id);
    }

    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepository;
        private IUserService _userService;

        public ProjectService(IProjectRepository projectRepository, IUserService userService)
        {
            _projectRepository = projectRepository;
            _userService = userService;
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public async Task<IEnumerable<Project>> GetAllByUser(string email)
        {
            var user = await _userService.GetByEmail(email);          

            return await _projectRepository.GetAllByUser(user.Id);
        }

        public async Task<Project> GetById(Guid id)
        {
            var user = await _projectRepository.GetById(id);

            if (user == null)
                throw new KeyNotFoundException("Project not found");

            return user;
        }

        public async Task<Guid> Create(ProjectCreateRequest model)
        {
            var user = await _userService.GetByEmail(model.UserEmail!);

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
            var project = await _projectRepository.GetById(id);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            project.Name = model.Name;
            project.UserId = model.UserId;

            await _projectRepository.Update(project);
        }

        public async Task Delete(Guid id)
        {
            await _projectRepository.Delete(id);
        }
    }
}
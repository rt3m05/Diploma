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
        Task<Guid> Create(ProjectCreateRequest model);
        Task Update(Guid id, ProjectUpdateRequest model);
        Task Delete(Guid id);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;

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
            var project = await _projectRepository.GetById(id);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            return project;
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
            if(model.UserEmail == null && model.Name == null)
                throw new EmptyModelException("Model was empty");

            var project = await _projectRepository.GetById(id);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            if (model.UserEmail != null)
            {
                var user = await _userService.GetByEmail(model.UserEmail);
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                project.UserId = user.Id;
            }

            if(model.Name != null)
                project.Name = model.Name; 

            await _projectRepository.Update(project);
        }

        //TODO: Add delete all info(tabs, tiles ...)
        public async Task Delete(Guid id)
        {
            if(await _projectRepository.GetById(id) != null)
                await _projectRepository.Delete(id);
            else
                throw new KeyNotFoundException("Project not found");
        }
    }
}
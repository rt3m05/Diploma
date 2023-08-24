using webapi.DB.Repositories;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.Tab;

namespace webapi.DB.Services
{
    public interface ITabService
    {
        Task<IEnumerable<Tab>> GetAll();
        Task<IEnumerable<Tab>> GetAllByProject(Guid projectId);
        Task<Tab> GetById(Guid id);
        Task<Guid> Create(TabCreateRequest model, Guid userId);
        Task Update(Guid id, TabUpdateRequest model);
        Task Delete(Guid id);
        Task DeleteAllByUser(Guid userId);
    }

    public class TabService : ITabService
    {
        private readonly ITabRepository _tabRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITileService _tileService;

        public TabService(ITabRepository tabRepository, IProjectRepository projectRepository, IUserRepository userRepository, ITileService tileService)
        {
            _tabRepository = tabRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _tileService = tileService;
        }

        public async Task<IEnumerable<Tab>> GetAll()
        {
            return await _tabRepository.GetAll();
        }

        public async Task<IEnumerable<Tab>> GetAllByProject(Guid projectId)
        {
            var project = await _projectRepository.GetById(projectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            return await _tabRepository.GetAllByProject(projectId);
        }

        public async Task<Tab> GetById(Guid id)
        {
            var tab = await _tabRepository.GetById(id);

            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

            return tab;
        }

        public async Task<Guid> Create(TabCreateRequest model, Guid userId)
        {
            var project = await _projectRepository.GetById(model.ProjectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            Tab tab = new()
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                UserId = userId,
                Name = model.Name,
                TimeStamp = DateTime.Now
            };

            await _tabRepository.Create(tab);

            return tab.Id;
        }

        public async Task Update(Guid id, TabUpdateRequest model)
        {
            if (model.Name == null)
                throw new EmptyModelException("Model was empty");

            var tab = await _tabRepository.GetById(id);
            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

            tab.Name = model.Name;

            await _tabRepository.Update(tab);
        }

        public async Task Delete(Guid id)
        {
            if (await _tabRepository.GetById(id) != null)
            {
                foreach(var tile in await _tileService.GetAllByTab(id))
                {
                    await _tileService.Delete(tile.Id);
                }

                await _tabRepository.Delete(id);
            }
            else
                throw new KeyNotFoundException("Tab not found");
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            if (await _userRepository.GetById(userId) != null)
            {
                await _tileService.DeleteAllByUser(userId);
                await _tabRepository.DeleteAllByUser(userId);
            }
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
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
        Task<Guid> Create(TabCreateRequest model);
        Task Update(Guid id, TabUpdateRequest model);
        Task Delete(Guid id);
    }

    public class TabService : ITabService
    {
        private readonly ITabRepository _tabRepository;
        private readonly IProjectService _projectService;

        public TabService(ITabRepository tabRepository, IProjectService projectService)
        {
            _tabRepository = tabRepository;
            _projectService = projectService;
        }

        public async Task<IEnumerable<Tab>> GetAll()
        {
            return await _tabRepository.GetAll();
        }

        public async Task<IEnumerable<Tab>> GetAllByProject(Guid projectId)
        {
            var project = await _projectService.GetById(projectId);

            return await _tabRepository.GetAllByProject(projectId);
        }

        public async Task<Tab> GetById(Guid id)
        {
            var tab = await _tabRepository.GetById(id);

            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

            return tab;
        }

        public async Task<Guid> Create(TabCreateRequest model)
        {
            var project = await _projectService.GetById(model.ProjectId);

            Tab tab = new()
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                Name = model.Name,
                TimeStamp = DateTime.Now
            };

            await _tabRepository.Create(tab);

            return tab.Id;
        }

        public async Task Update(Guid id, TabUpdateRequest model)
        {
            if (model.ProjectId == null && model.Name == null)
                throw new EmptyModelException("Model was empty");

            var tab = await _tabRepository.GetById(id);
            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

            if (model.ProjectId != null && model.ProjectId != Guid.Empty && model.ProjectId.HasValue)
            {
                var project = await _projectService.GetById(model.ProjectId.Value);
                if (project == null)
                    throw new KeyNotFoundException("Project not found");

                tab.ProjectId = project.Id;
            }

            if (model.Name != null)
                tab.Name = model.Name;

            await _tabRepository.Update(tab);
        }

        //TODO: Add delete all info(tiles, tiles items)
        public async Task Delete(Guid id)
        {
            if (await _tabRepository.GetById(id) != null)
                await _tabRepository.Delete(id);
            else
                throw new KeyNotFoundException("Tab not found");
        }
    }
}
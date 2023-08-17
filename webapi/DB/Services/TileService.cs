using webapi.DB.Repositories;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.Tile;

namespace webapi.DB.Services
{
    public interface ITileService
    {
        Task<IEnumerable<Tile>> GetAll();
        Task<IEnumerable<Tile>> GetAllByTab(Guid tabId);
        Task<Tile> GetById(Guid id);
        Task<Guid> Create(TileCreateRequest model, Guid userId);
        Task Update(Guid id, TileUpdateRequest model);
        Task Delete(Guid id);
    }

    public class TileService : ITileService
    {
        private readonly ITileRepository _tileRepository;
        private readonly ITabService _tabService;

        public TileService(ITileRepository tileRepository, ITabService tabService)
        {
            _tileRepository = tileRepository;
            _tabService = tabService;
        }

        public async Task<IEnumerable<Tile>> GetAll()
        {
            return await _tileRepository.GetAll();
        }

        public async Task<IEnumerable<Tile>> GetAllByTab(Guid tabId)
        {
            var tab = await _tabService.GetById(tabId);

            return await _tileRepository.GetAllByTab(tabId);
        }

        public async Task<Tile> GetById(Guid id)
        {
            var tile = await _tileRepository.GetById(id);

            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            return tile;
        }

        public async Task<Guid> Create(TileCreateRequest model, Guid userId)
        {
            var tab = await _tabService.GetById(model.TabId);

            Tile tile = new()
            {
                Id = Guid.NewGuid(),
                TabId = tab.Id,
                UserId = userId,
                Name = model.Name,
                X = model.X,
                Y = model.Y,
                H = model.H,
                W = model.W,
                TimeStamp = DateTime.Now
            };

            await _tileRepository.Create(tile);

            return tile.Id;
        }

        public async Task Update(Guid id, TileUpdateRequest model)
        {
            if (model.isEmpty())
                throw new EmptyModelException("Model was empty");

            var tile = await _tileRepository.GetById(id);
            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            if (model.Name != null)
                tile.Name = model.Name;

            if (model.X != null && model.X.HasValue)
                tile.X = model.X.Value;

            if (model.Y != null && model.Y.HasValue)
                tile.Y = model.Y.Value;

            if (model.H != null && model.H.HasValue)
                tile.H = model.H.Value;

            if (model.W != null && model.W.HasValue)
                tile.W = model.W.Value;

            await _tileRepository.Update(tile);
        }

        //TODO: Add delete all info(tiles items)
        public async Task Delete(Guid id)
        {
            if (await _tileRepository.GetById(id) != null)
                await _tileRepository.Delete(id);
            else
                throw new KeyNotFoundException("Tile not found");
        }
    }
}

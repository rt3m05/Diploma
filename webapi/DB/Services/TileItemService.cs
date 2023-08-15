using webapi.DB.Repositories;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.TileItem;

namespace webapi.DB.Services
{
    public interface ITileItemService
    {
        Task<IEnumerable<TileItem>> GetAll();
        Task<IEnumerable<TileItem>> GetAllByTile(Guid tileId);
        Task<TileItem> GetById(Guid id);
        Task<Guid> Create(TileItemCreateRequest model);
        Task Update(Guid id, TileItemUpdateRequest model);
        Task Delete(Guid id);
    }

    public class TileItemService : ITileItemService
    {
        private readonly ITileItemRepository _tileItemRepository;
        private readonly ITileService _tileService;

        public TileItemService(ITileItemRepository tileItemRepository, ITileService tileService)
        {
            _tileItemRepository = tileItemRepository;
            _tileService = tileService;
        }

        public async Task<IEnumerable<TileItem>> GetAll()
        {
            return await _tileItemRepository.GetAll();
        }

        public async Task<IEnumerable<TileItem>> GetAllByTile(Guid tileId)
        {
            var tile = await _tileService.GetById(tileId);

            return await _tileItemRepository.GetAllByTile(tileId);
        }

        public async Task<TileItem> GetById(Guid id)
        {
            var tileItem = await _tileItemRepository.GetById(id);

            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

            return tileItem;
        }

        public async Task<Guid> Create(TileItemCreateRequest model)
        {
            var tile = await _tileService.GetById(model.TileId);

            TileItem tileItem = new()
            {
                Id = Guid.NewGuid(),
                TileId = tile.Id,
                Content = model.Content,
                Type = model.Type,
                TimeStamp = DateTime.Now
            };

            await _tileItemRepository.Create(tileItem);

            return tileItem.Id;
        }

        public async Task Update(Guid id, TileItemUpdateRequest model)
        {
            if (model.TileId == null && model.Content == null && model.Type == null)
                throw new EmptyModelException("Model was empty");

            var tileItem = await _tileItemRepository.GetById(id);
            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

            if (model.TileId != null && model.TileId != Guid.Empty && model.TileId.HasValue)
            {
                var tile = await _tileService.GetById(model.TileId.Value);
                if (tile == null)
                    throw new KeyNotFoundException("Tile not found");

                tileItem.TileId = tile.Id;
            }

            if (model.Content != null)
                tileItem.Content = model.Content;

            if (model.Type != null && model.Type.HasValue)
                tileItem.Type = model.Type.Value;

            await _tileItemRepository.Update(tileItem);
        }

        public async Task Delete(Guid id)
        {
            if (await _tileItemRepository.GetById(id) != null)
                await _tileItemRepository.Delete(id);
            else
                throw new KeyNotFoundException("Tile Item not found");
        }
    }
}
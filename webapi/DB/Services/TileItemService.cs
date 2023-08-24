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
        Task<Guid> Create(TileItemCreateRequest model, Guid userId);
        Task Update(Guid id, TileItemUpdateRequest model);
        Task Delete(Guid id);
        Task DeleteAllByUser(Guid userId);
    }

    public class TileItemService : ITileItemService
    {
        private readonly ITileItemRepository _tileItemRepository;
        private readonly ITileRepository _tileRepository;
        private readonly IUserRepository _userRepository;

        public TileItemService(ITileItemRepository tileItemRepository, ITileRepository tileRepository, IUserRepository userRepository)
        {
            _tileItemRepository = tileItemRepository;
            _tileRepository = tileRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TileItem>> GetAll()
        {
            return await _tileItemRepository.GetAll();
        }

        public async Task<IEnumerable<TileItem>> GetAllByTile(Guid tileId)
        {
            var tile = await _tileRepository.GetById(tileId);
            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            return await _tileItemRepository.GetAllByTile(tileId);
        }

        public async Task<TileItem> GetById(Guid id)
        {
            var tileItem = await _tileItemRepository.GetById(id);

            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

            return tileItem;
        }

        public async Task<Guid> Create(TileItemCreateRequest model, Guid userId)
        {
            var tile = await _tileRepository.GetById(model.TileId);
            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            TileItem tileItem = new()
            {
                Id = Guid.NewGuid(),
                TileId = tile.Id,
                UserId = userId,
                Content = model.Content,
                Type = model.Type,
                TimeStamp = DateTime.Now
            };

            await _tileItemRepository.Create(tileItem);

            return tileItem.Id;
        }

        public async Task Update(Guid id, TileItemUpdateRequest model)
        {
            if (model.Content == null && model.Type == null)
                throw new EmptyModelException("Model was empty");

            var tileItem = await _tileItemRepository.GetById(id);
            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

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

        public async Task DeleteAllByUser(Guid userId)
        {
            if (await _userRepository.GetById(userId) != null)
                await _tileItemRepository.DeleteAllByUser(userId);
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
using Microsoft.Extensions.Options;
using webapi.DB.Repositories;
using webapi.DB.Services.Settings;
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
        Task<IEnumerable<TileItem>> GetAllByTileWithImage(Guid tileId);
        Task<TileItem> GetByIdWithImage(Guid id);
        Task<Guid> Create(TileItemCreateRequest model, Guid userId);
        Task Update(Guid id, TileItemUpdateRequest model);
        Task ChangePosition(Guid id, Guid tileId, byte newPos, byte oldPos);
        Task Delete(Guid id);
        Task DeleteWithPositionChange(Guid id, Guid tileId, byte pos);
        Task DeleteAllByUser(Guid userId);
    }

    public class TileItemService : ITileItemService
    {
        private TileItemServiceSettings _settings;
        private readonly ITileItemRepository _tileItemRepository;
        private readonly ITileRepository _tileRepository;
        private readonly IUserRepository _userRepository;

        public TileItemService(IOptions<TileItemServiceSettings> settings, ITileItemRepository tileItemRepository, ITileRepository tileRepository, IUserRepository userRepository)
        {
            _settings = settings.Value;
            _tileItemRepository = tileItemRepository;
            _tileRepository = tileRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TileItem>> GetAll()
        {
            var all = await _tileItemRepository.GetAll();
            foreach(var item in all) 
            {
                item.Image = File.ReadAllBytes(_settings.dir + "/" + item.UserId.ToString() + "/" + item.Content);
            }
            return all;
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

        public async Task<IEnumerable<TileItem>> GetAllByTileWithImage(Guid tileId)
        {
            var tile = await _tileRepository.GetById(tileId);
            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            var all = await _tileItemRepository.GetAllByTile(tileId);
            foreach (var item in all)
            {
                item.Image = File.ReadAllBytes(_settings.dir + "/" + item.UserId.ToString() + "/" + item.Content);
            }
            return all;
        }

        public async Task<TileItem> GetByIdWithImage(Guid id)
        {
            var tileItem = await _tileItemRepository.GetById(id);

            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

            tileItem.Image = File.ReadAllBytes(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content);

            return tileItem;
        }

        public async Task<Guid> Create(TileItemCreateRequest model, Guid userId)
        {
            var tile = await _tileRepository.GetById(model.TileId);
            if (tile == null)
                throw new KeyNotFoundException("Tile not found");

            string content = model.Content!;

            if (model.File != null && model.File.Length > 0)
            {
                var strs = model.File.FileName.Split('.');
                content = Guid.NewGuid().ToString() + "." + strs.Last();

                if (!Directory.Exists(_settings.dir + "/" + userId.ToString()))
                {
                    Directory.CreateDirectory(_settings.dir + "/" + userId.ToString());
                }
                if (!File.Exists(_settings.dir + "/" + userId.ToString() + "/" + content))
                {
                    using(var stream = File.Create(_settings.dir + "/" + userId.ToString() + "/" + content))
                    {
                        await model.File.CopyToAsync(stream);
                        stream.Flush();
                    }
                }
            }

            TileItem tileItem = new()
            {
                Id = Guid.NewGuid(),
                TileId = tile.Id,
                UserId = userId,
                Content = content,
                Type = model.Type,
                Position = model.Position,
                IsDone = model.IsDone,
                TimeStamp = DateTime.Now
            };

            await _tileItemRepository.CreateWithPositionChange(tileItem);

            return tileItem.Id;
        }

        public async Task Update(Guid id, TileItemUpdateRequest model)
        {
            if (model.isEmpty())
                throw new EmptyModelException("Model was empty");

            var tileItem = await _tileItemRepository.GetById(id);
            if (tileItem == null)
                throw new KeyNotFoundException("Tile Item not found");

            if (model.Content != null)
                tileItem.Content = model.Content;

            if (model.File != null && model.File.Length > 0)
            {
                if(tileItem.Content != null && tileItem.Type == TileItemTypes.Image && File.Exists(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content))
                {
                    File.Delete(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content);
                }

                var strs = model.File.FileName.Split('.');
                tileItem.Content = Guid.NewGuid().ToString() + "." + strs.Last();

                if (!Directory.Exists(_settings.dir + "/" + tileItem.UserId.ToString()))
                {
                    Directory.CreateDirectory(_settings.dir + "/" + tileItem.UserId.ToString());
                }
                if (!File.Exists(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content))
                {
                    using (var stream = File.Create(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content))
                    {
                        await model.File.CopyToAsync(stream);
                        stream.Flush();
                    }
                }
            }

            if (model.Type != null && model.Type.HasValue)
                tileItem.Type = model.Type.Value;

            if (model.IsDone != null && model.IsDone.HasValue)
                tileItem.IsDone = model.IsDone.Value;

            if (model.isOnlyPosition() && model.Position.HasValue)
                await _tileItemRepository.ChangePosition(tileItem.Id, tileItem.TileId, model.Position.Value, tileItem.Position);
            else if (!model.isOnlyPosition() && model.Position.HasValue)
                await _tileItemRepository.UpdateWithPositionChange(tileItem, model.Position.Value);
            else
                await _tileItemRepository.Update(tileItem);
        }

        public async Task ChangePosition(Guid id, Guid tileId, byte newPos, byte oldPos)
        {
            if (await _tileItemRepository.GetById(id) != null)
                await _tileItemRepository.ChangePosition(id, tileId, newPos, oldPos);
            else
                throw new KeyNotFoundException("Tile Item not found");
        }

        public async Task Delete(Guid id)
        {
            var tileItem = await _tileItemRepository.GetById(id);
            if (tileItem != null)
            {
                if (tileItem.Content != null && tileItem.Type == TileItemTypes.Image && File.Exists(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content))
                {
                    File.Delete(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content);
                }
                await _tileItemRepository.Delete(id);
            }
            else
                throw new KeyNotFoundException("Tile Item not found");
        }

        public async Task DeleteWithPositionChange(Guid id, Guid tileId, byte pos)
        {
            var tileItem = await _tileItemRepository.GetById(id);
            if (tileItem != null && await _tileRepository.GetById(tileId) != null)
            {
                if (tileItem.Content != null && tileItem.Type == TileItemTypes.Image && File.Exists(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content))
                {
                    File.Delete(_settings.dir + "/" + tileItem.UserId.ToString() + "/" + tileItem.Content);
                }
                await _tileItemRepository.DeleteWithPositionChange(id, tileId, pos);
            }
            else
                throw new KeyNotFoundException("Tile Item or Tile not found");
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            if (await _userRepository.GetById(userId) != null)
            {
                if(Directory.Exists(_settings.dir + "/" + userId.ToString()))
                    Directory.Delete(_settings.dir + "/" + userId.ToString(), true);

                await _tileItemRepository.DeleteAllByUser(userId);
            }
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}
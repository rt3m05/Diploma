﻿using webapi.DB.Repositories;
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
        Task DeleteAllByUser(Guid userId);
    }

    public class TileService : ITileService
    {
        private readonly ITileRepository _tileRepository;
        private readonly ITabRepository _tabRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITileItemService _tileItemService;

        public TileService(ITileRepository tileRepository, ITabRepository tabRepository, IUserRepository userRepository, ITileItemService tileItemService)
        {
            _tileRepository = tileRepository;
            _tabRepository = tabRepository;
            _userRepository = userRepository;
            _tileItemService = tileItemService;
        }

        public async Task<IEnumerable<Tile>> GetAll()
        {
            return await _tileRepository.GetAll();
        }

        public async Task<IEnumerable<Tile>> GetAllByTab(Guid tabId)
        {
            var tab = await _tabRepository.GetById(tabId);
            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

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
            var tab = await _tabRepository.GetById(model.TabId);
            if (tab == null)
                throw new KeyNotFoundException("Tab not found");

            var tiles = await GetAllByTab(tab.Id);

            foreach (var t in tiles)
            {
                if (t.Position == model.Position)
                    throw new PositionIsUsedException("Position is used");
            }

            Tile tile = new()
            {
                Id = Guid.NewGuid(),
                TabId = tab.Id,
                UserId = userId,
                Name = model.Name,
                Position = model.Position,
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

            var tiles = await GetAllByTab(tile.TabId);

            foreach (var t in tiles)
            {
                if (t.Position == model.Position && t.Id != id)
                    throw new PositionIsUsedException("Position is used");
            }

            if (model.Name != null)
                tile.Name = model.Name;

            if (model.Position != null && model.Position.HasValue)
                tile.Position = model.Position.Value;

            await _tileRepository.Update(tile);
        }

        public async Task Delete(Guid id)
        {
            if (await _tileRepository.GetById(id) != null)
            {
                foreach(var tileItem in await _tileItemService.GetAllByTile(id))
                {
                    await _tileItemService.Delete(tileItem.Id);
                }

                await _tileRepository.Delete(id);
            }
            else
                throw new KeyNotFoundException("Tile not found");
        }

        public async Task DeleteAllByUser(Guid userId)
        {
            if (await _userRepository.GetById(userId) != null)
            {
                await _tileItemService.DeleteAllByUser(userId);
                await _tileRepository.DeleteAllByUser(userId);
            }
            else
                throw new KeyNotFoundException("User not found");
        }
    }
}

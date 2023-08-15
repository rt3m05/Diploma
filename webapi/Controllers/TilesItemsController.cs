﻿using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.DTO.TilesItems;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.TileItem;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesItemsController : ControllerBase
    {
        private readonly ITileItemService _tileItemService;
        private readonly ILogger<TilesItemsController> _logger;
        private readonly IWebHostEnvironment _env;

        public TilesItemsController(ITileItemService tileItemService, ILogger<TilesItemsController> logger, IWebHostEnvironment env)
        {
            _tileItemService = tileItemService;
            _logger = logger;
            _env = env;
        }

        // GET api/TilesItems/GetTypes
        [HttpGet("GetTypes")]
        public IActionResult GetTypes()
        {
            try
            {
                return Ok(Enum.GetValues(typeof(TileItemTypes)).Cast<TileItemTypes>().Select(tit => tit.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // GET: api/TilesItems/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _tileItemService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // GET: api/TilesItems
        [HttpGet]
        public async Task<IActionResult> GetAllByTile(Guid tileId)
        {
            try
            {
                var tilesItems = await _tileItemService.GetAllByTile(tileId);

                _logger.LogInformation("Get tiles items by tile successfull. Tile Id: " + tileId.ToString());
                return Ok(tilesItems.Select(ti => new TileItemAllInfo(ti)));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Tile Id: " + tileId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // GET api/TilesItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var tileItem = await _tileItemService.GetById(id);

                return Ok(tileItem);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Id: " + id);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // POST api/TilesItems
        [HttpPost]
        public async Task<IActionResult> Create(TileItemCreateRequest model)
        {
            Guid id = Guid.Empty;

            bool typeIsValid = false;
            foreach (var item in Enum.GetValues(typeof(TileItemTypes)).Cast<TileItemTypes>().Select(tit => tit.ToString()))
            {
                if (item == model.Type.ToString())
                {
                    typeIsValid = true;
                    break;
                }
            }
            if(!typeIsValid)
            {
                _logger.LogError("Wrong type. Please see types in TileItemTypes");
                return BadRequest(new { message = "Wrong type. Please see types in TileItemTypes" });
            }

            try
            {
                id = await _tileItemService.Create(model);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            if (id == Guid.Empty)
            {
                _logger.LogError("After create tile item Id was empty.");
                return StatusCode(500);
            }

            _logger.LogInformation("Tile Item created. Id: " + id.ToString());
            return Ok(new { message = "Tile Item created" });
        }

        // PUT api/TilesItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TileItemUpdateRequest model)
        {
            if(model.Type != null && model.Type.HasValue)
            {
                bool typeIsValid = false;
                foreach (var item in Enum.GetValues(typeof(TileItemTypes)).Cast<TileItemTypes>().Select(tit => tit.ToString()))
                {
                    if (item == model.Type.Value.ToString())
                    {
                        typeIsValid = true;
                        break;
                    }
                }
                if (!typeIsValid)
                {
                    _logger.LogError("Wrong type. Please see types in TileItemTypes");
                    return BadRequest(new { message = "Wrong type. Please see types in TileItemTypes" });
                }
            }

            try
            {
                await _tileItemService.Update(id, model);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Id: " + id.ToString());
                return BadRequest(new { message = ex.Message });
            }
            catch (EmptyModelException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("Tile Item updated. Id: " + id.ToString());
            return Ok(new { message = "Tile Item updated" });
        }

        // DELETE api/TilesItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _tileItemService.Delete(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Id: " + id.ToString());
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("Tile Item deleted. Id: " + id.ToString());
            return Ok(new { message = "Tile Item deleted" });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.DB.Services;
using webapi.DTO.TilesItems;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.TileItem;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TilesItemsController : ControllerBase
    {
        private readonly ITileItemService _tileItemService;
        private readonly ITileService _tileService;
        private readonly IUserService _userService;
        private readonly ILogger<TilesItemsController> _logger;
        private readonly IWebHostEnvironment _env;

        public TilesItemsController(ITileItemService tileItemService, ITileService tileService, IUserService userService, ILogger<TilesItemsController> logger, IWebHostEnvironment env)
        {
            _tileItemService = tileItemService;
            _tileService = tileService;
            _userService = userService;
            _logger = logger;
            _env = env;
        }

        // GET api/TilesItems/GetTypes
        [HttpGet("GetTypes")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                var tilesItems = await _tileItemService.GetAllByTileWithImage(tileId);

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
                var tileItem = await _tileItemService.GetByIdWithImage(id);

                return Ok(new TileItemAllInfo(tileItem));
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
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);
                var tile = await _tileService.GetById(model.TileId);

                if (tile.UserId == user.Id)
                    id = await _tileItemService.Create(model, user.Id);
                else
                    return BadRequest(new { message = "This is not your tile." });

                if (id == Guid.Empty)
                    throw new Exception("After create tile item Id was empty.");
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
                var tileItem = await _tileItemService.GetById(id);

                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);

                if (tileItem.UserId == user.Id)
                    await _tileItemService.Update(id, model);
                else
                    return BadRequest(new { message = "This is not your tile item." });
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
                var tileItem = await _tileItemService.GetById(id);

                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);

                if (tileItem.UserId == user.Id)
                    await _tileItemService.DeleteWithPositionChange(id, tileItem.TileId, tileItem.Position);
                else
                    return BadRequest(new { message = "This is not your tile item." });
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

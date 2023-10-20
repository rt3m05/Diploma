using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.DB.Services;
using webapi.DTO.Tiles;
using webapi.Exceptions;
using webapi.Requests.Tile;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TilesController : ControllerBase
    {
        private readonly ITileService _tileService;
        private readonly ITabService _tabService;
        private readonly IUserService _userService;
        private readonly ILogger<TilesController> _logger;
        private readonly IWebHostEnvironment _env;

        public TilesController(ITileService tileService, ITabService tabService, IUserService userService, ILogger<TilesController> logger, IWebHostEnvironment env)
        {
            _tileService = tileService;
            _tabService = tabService;
            _userService = userService;
            _logger = logger;
            _env = env;
        }

        // GET: api/Tiles/all
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _tileService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // GET: api/Tiles
        [HttpGet]
        public async Task<IActionResult> GetAllByTab(Guid tabId)
        {
            try
            {
                var tiles = await _tileService.GetAllByTab(tabId);

                return Ok(tiles.Select(t => new TileAllInfo(t)));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Tab Id: " + tabId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // GET api/Tiles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var tile = await _tileService.GetById(id);

                return Ok(new TileAllInfo(tile));
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

        // POST api/Tiles
        [HttpPost]
        public async Task<IActionResult> Create(TileCreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);
                var tab = await _tabService.GetById(model.TabId);

                if(tab.UserId == user.Id)
                    id = await _tileService.Create(model, user.Id);
                else
                    return BadRequest(new { message = "This is not your tab." });

                if (id == Guid.Empty)
                    throw new Exception("After create tile Id was empty.");
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

            _logger.LogInformation("Tile created. Id: " + id.ToString());
            return Ok(new { message = "Tile created" });
        }

        // PUT api/Tiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TileUpdateRequest model)
        {
            try
            {
                var tile = await _tileService.GetById(id);

                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);

                if (tile.UserId == user.Id)
                    await _tileService.Update(id, model);
                else
                    return BadRequest(new { message = "This is not your tile." });
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

            _logger.LogInformation("Tile updated. Id: " + id.ToString());
            return Ok(new { message = "Tile updated" });
        }

        // DELETE api/Tiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var tile = await _tileService.GetById(id);

                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmail(email);

                if (tile.UserId == user.Id)
                    await _tileService.DeleteWithPositionChange(id, tile.TabId, tile.Position);
                else
                    return BadRequest(new { message = "This is not your tile." });
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

            _logger.LogInformation("Tile deleted. Id: " + id.ToString());
            return Ok(new { message = "Tile deleted" });
        }
    }
}

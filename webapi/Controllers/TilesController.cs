using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.DTO.Tiles;
using webapi.Exceptions;
using webapi.Requests.Tile;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly ITileService _tileService;
        private readonly ILogger<TilesController> _logger;
        private readonly IWebHostEnvironment _env;

        public TilesController(ITileService tileService, ILogger<TilesController> logger, IWebHostEnvironment env)
        {
            _tileService = tileService;
            _logger = logger;
            _env = env;
        }

        // GET: api/Tiles/all
        [HttpGet("all")]
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

                _logger.LogInformation("Get tiles by tab successfull. Tab Id: " + tabId.ToString());
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

                return Ok(tile);
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
                id = await _tileService.Create(model);
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
                _logger.LogError("After create tile Id was empty.");
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
                await _tileService.Update(id, model);
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
                await _tileService.Delete(id);
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

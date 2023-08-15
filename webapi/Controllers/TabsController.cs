using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.DTO.Tabs;
using webapi.Exceptions;
using webapi.Requests.Tab;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabsController : ControllerBase
    {
        private readonly ITabService _tabService;
        private readonly ILogger<TabsController> _logger;
        private readonly IWebHostEnvironment _env;

        public TabsController(ITabService tabService, ILogger<TabsController> logger, IWebHostEnvironment env)
        {
            _tabService = tabService;
            _logger = logger;
            _env = env;
        }

        // GET: api/Tabs/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _tabService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // GET: api/Tabs
        [HttpGet]
        public async Task<IActionResult> GetAllByProject(Guid projectId)
        {
            try
            {
                var tabs = await _tabService.GetAllByProject(projectId);

                _logger.LogInformation("Get tabs by project successfull. Project Id: " + projectId.ToString());
                return Ok(tabs.Select(t => new TabAllInfo(t)));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Project Id: " + projectId);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // GET api/Tabs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var tab = await _tabService.GetById(id);

                return Ok(tab);
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

        // POST api/Tabs
        [HttpPost]
        public async Task<IActionResult> Create(TabCreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                id = await _tabService.Create(model);
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
                _logger.LogError("After create tab Id was empty.");
                return StatusCode(500);
            }

            _logger.LogInformation("Tab created. Id: " + id.ToString());
            return Ok(new { message = "Tab created" });
        }

        // PUT api/Tabs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TabUpdateRequest model)
        {
            try
            {
                await _tabService.Update(id, model);
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

            _logger.LogInformation("Tab updated. Id: " + id.ToString());
            return Ok(new { message = "Tab updated" });
        }

        // DELETE api/Tabs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _tabService.Delete(id);
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

            _logger.LogInformation("Tab deleted. Id: " + id.ToString());
            return Ok(new { message = "Tab deleted" });
        }
    }
}
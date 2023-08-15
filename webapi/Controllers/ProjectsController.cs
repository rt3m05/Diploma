using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.DTO.Projects;
using webapi.Exceptions;
using webapi.Requests.Project;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IWebHostEnvironment _env;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger, IWebHostEnvironment env)
        {
            _projectService = projectService;
            _logger = logger;
            _env = env;
        }

        // GET: api/Projects/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _projectService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> GetAllByUser(string userEmail)
        {
            try
            {
                var projects = await _projectService.GetAllByUser(userEmail);

                _logger.LogInformation("Get projects by user successfull. User Email: " + userEmail);
                return Ok(projects.Select(p => new ProjectAllInfo(p)));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " User Email: " + userEmail);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // GET api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var project = await _projectService.GetById(id);

                return Ok(new ProjectAllInfo(project));
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

        // POST api/Projects
        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                id = await _projectService.Create(model);
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
                _logger.LogError("After create project Id was empty.");
                return StatusCode(500);
            }

            _logger.LogInformation("Project created. Id: " + id.ToString());
            return Ok(new { message = "Project created" });
        }

        // PUT api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProjectUpdateRequest model)
        {
            try
            {
                await _projectService.Update(id, model);
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

            _logger.LogInformation("Project updated. Id: " + id.ToString());
            return Ok(new { message = "Project updated" });
        }

        // DELETE api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _projectService.Delete(id);
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

            _logger.LogInformation("Project deleted. Id: " + id.ToString());
            return Ok(new { message = "Project deleted" });
        }
    }
}

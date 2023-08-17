using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.DB.Services;
using webapi.DTO.Projects;
using webapi.Exceptions;
using webapi.Requests.Project;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IWebHostEnvironment _env;

        public ProjectsController(IProjectService projectService, IUserService userService, ILogger<ProjectsController> logger, IWebHostEnvironment env)
        {
            _projectService = projectService;
            _userService = userService;
            _logger = logger;
            _env = env;
        }

        // GET: api/Projects/all
        [HttpGet("all")]
        [AllowAnonymous]
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
        public async Task<IActionResult> GetAllByUser()
        {
            string? email = null;
            try
            {
                email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var projects = await _projectService.GetAllByUser(email);

                return Ok(projects.Select(p => new ProjectAllInfo(p, true)));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " User Email: " + email);
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
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var project = await _projectService.GetById(id);
                var user = await _userService.GetByEmail(email);

                var projectInfo = new ProjectAllInfo(project, project.UserId == user.Id);

                return Ok(projectInfo);
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
        }

        // POST api/Projects
        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                id = await _projectService.Create(model, email);

                if (id == Guid.Empty)
                    throw new Exception("After create project Id was empty.");
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

            _logger.LogInformation("Project created. Id: " + id.ToString());
            return Ok(new { message = "Project created" });
        }

        // PUT api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProjectUpdateRequest model)
        {
            try
            {
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var project = await _projectService.GetById(id);
                var user = await _userService.GetByEmail(email);

                if(project.UserId == user.Id)
                    await _projectService.Update(id, model);
                else
                    return BadRequest(new { message = "This is not your project." });
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
                var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var project = await _projectService.GetById(id);
                var user = await _userService.GetByEmail(email);

                if (project.UserId == user.Id)
                    await _projectService.Delete(id);
                else
                    return BadRequest(new { message = "This is not your project." });
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

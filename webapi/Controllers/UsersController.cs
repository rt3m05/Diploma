using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.Exceptions;
using webapi.Requests.User;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _env;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IWebHostEnvironment env)
        {
            _userService = userService;
            _logger = logger;
            _env = env;
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _userService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                id = await _userService.Create(model);
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            if(id == Guid.Empty)
            {
                _logger.LogError("After create user Id was empty.");
                return StatusCode(500);
            }

            _logger.LogInformation("User created. Id: " + id.ToString());
            return Ok(new { message = "User created" });
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserUpdateRequest model)
        {
            try
            {
                await _userService.Update(id, model);
            } 
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Id: " + id.ToString());
                return BadRequest(new { message = ex.Message });
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("User updated. Id: " + id.ToString());
            return Ok(new { message = "User updated" });
        }

        // DELETE api/users
        [HttpDelete]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                await _userService.Delete(email);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + email);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("User deleted. Email: " + email);
            return Ok(new { message = "User deleted" });
        }
    }
}

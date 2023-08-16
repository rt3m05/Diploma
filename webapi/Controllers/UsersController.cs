using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.Auth;
using webapi.DB.Services;
using webapi.Exceptions;
using webapi.Requests.User;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IWebHostEnvironment env, IAuthService authService)
        {
            _userService = userService;
            _logger = logger;
            _env = env;
            _authService = authService;
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

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest model)
        {
            try
            {
                var token = await _authService.GetToken(model);

                //HttpContext.User ??
                
                _logger.LogInformation("User authenticated. Email: " + model.Email);
                return Ok(token);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + model.Email);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidPasswordException ex)
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

        // GET: api/users/logout
        //[HttpGet("logout")]
        //public IActionResult Logout()
        //{
        //    try
        //    {
        //        SignOut(JwtBearerDefaults.AuthenticationScheme);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500);
        //    }
        //}

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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(string email, UserUpdateRequest model)
        {
            try
            {
                
                await _userService.Update(email, model);
            } 
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + email);
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

            _logger.LogInformation("User updated. Email: " + email);
            return Ok(new { message = "User updated" });
        }

        // DELETE api/users
        [HttpDelete]
        [Authorize]
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

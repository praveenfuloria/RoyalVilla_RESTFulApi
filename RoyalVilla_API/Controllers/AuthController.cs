using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;
using RoyalVilla_API.Services;

namespace RoyalVilla_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody]RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                if (registrationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration Data is Required", null));
                }

                if (await _authService.IsEmailExistsAsync(registrationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email {registrationRequestDTO.Email} already exists"));
                }

                var userDTO = await _authService.RegisterAsync(registrationRequestDTO);

                if (userDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration Failed", null));
                }
                var apiResponse = ApiResponse<UserDTO>.CreatedAt("User Created Successfully", userDTO);
                return CreatedAtAction(nameof(Register), apiResponse);
            }
            catch (Exception ex) {
                var errorResponse = ApiResponse<object>.Error($"An error occured while registering the user", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login Data is Required", null));
                }

                var loginResponseDTO = await _authService.LoginAsync(loginRequestDTO);

                if (loginResponseDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login Failed", null));
                }
                var apiResponse = ApiResponse<LoginResponseDTO>.Ok("User Logged In Successfully", loginResponseDTO);
                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error($"An error occured while login the user", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}

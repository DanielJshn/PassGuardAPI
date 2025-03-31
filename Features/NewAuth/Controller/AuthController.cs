using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDataRegistrationDto userDataRegistrationDto)
        {
            try
            {
                var result = await _authService.CreateNewAccountAsync(userDataRegistrationDto);
                return Ok(new ApiResponse(true, data: result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse(false, ex.Message));
            }
            catch (Exception ex)
            {

                return StatusCode(500, new ApiResponse(false, ex.Message));
            }
        }




        
    }
}
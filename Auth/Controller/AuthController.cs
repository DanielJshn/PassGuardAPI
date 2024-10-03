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
        public async Task<IActionResult> Register(userForRegistration userForRegistration)
        {
            string token;
            try
            {
                // await _authService.ValidateRegistrationDataAsync(userForRegistration);
                await _authService.CheckUserExistsAsync(userForRegistration);
                token = await _authService.GenerateTokenAsync(userForRegistration);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
            return Ok(new ApiResponse(success: true, data: new { Token = token }));
        }
    }
}
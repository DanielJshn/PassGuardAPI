using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IVerifyService _verifyService;
        public AuthController(IAuthService authService, IVerifyService verifyService)
        {
            _verifyService = verifyService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDataRegistrationDto userDataRegistrationDto)
        {
            try
            {
                var result = await _authService.CreateNewAccountAsync(userDataRegistrationDto);
                await _verifyService.SendOTP(result.email, result.id);
                return Ok(new ApiResponse(true, data: result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(false, ex.Message));
            }
        }

    }
}
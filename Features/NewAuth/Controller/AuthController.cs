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
                return Ok(new ApiResponse(success: true,  data: null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
            
        }
    }
}
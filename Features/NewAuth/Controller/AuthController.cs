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
                await _verifyService.SendOTP(result.email);
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CheckOTP(OTPdto otp)
        {
            try
            {
                await _verifyService.CheckOTP(otp);
                return Ok(new ApiResponse(true, data: Ok()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(false, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> ResendOTP(OTPresendDto otp)
        {
            try
            {
                await _verifyService.ResendOTP(otp.email);
                return Ok(new ApiResponse(true, data: Ok()));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(false, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("loginStart")]
        public async Task<IActionResult> LoginStart(UserEmailDto userEmailDto)
        {
            try
            {
                var result = await _authService.StartLoginAsync(userEmailDto.email);
                return Ok(new ApiResponse(true, data: result));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(false, ex.Message));
            }
        }
    }
}
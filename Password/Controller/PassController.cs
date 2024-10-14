using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class PassController : ControllerBase
    {

        private readonly IPassService _passwordService;

        public PassController(IPassService passwordService)
        {
            _passwordService = passwordService;
        }


        [HttpPost]
        public async Task<ActionResult<PasswordDto>> PostPassword([FromBody] PasswordDto password)
        {
            try
            {
                if (password == null)
                {
                    return BadRequest("Password data is null.");
                }
                var identity = await _passwordService.GetUserByTokenAsync(User);
                var createdPassword = await _passwordService.CreateAsync(password, identity.id);
                return Ok(new ApiResponse(success: true, data: createdPassword));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetPasswords()
        {
            try
            {
                var identity = await _passwordService.GetUserByTokenAsync(User);
                List<PasswordResponsDto> getPassword = await _passwordService.GetAllPasswordsForUserAsync(identity.id);
                return Ok(new ApiResponse(success: true, data: getPassword));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }


        [HttpPut("{passwordId}")]
        public async Task<ActionResult<PasswordDto>> UpdatePassword(Guid passwordId, [FromBody] PasswordDto dataInput)
        {
            try
            {
                var identity = await _passwordService.GetUserByTokenAsync(User);
                PasswordDto updatedPassword = await _passwordService.UpdatePassword(identity.id, passwordId, dataInput);
                return Ok(updatedPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
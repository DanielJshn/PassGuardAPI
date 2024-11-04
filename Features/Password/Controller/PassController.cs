using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class PassController : ControllerBase
    {

        private readonly IPassService _passwordService;
        private readonly IIdentityUser _identity;

        public PassController(IPassService passwordService, IIdentityUser identity)
        {
            _passwordService = passwordService;
            _identity = identity;
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
                var identity = await _identity.GetUserByTokenAsync(User);
                var createdPassword = await _passwordService.CreateAsync(password, identity.id);
                Console.WriteLine(createdPassword);
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
                var identity = await _identity.GetUserByTokenAsync(User);
                List<PasswordResponsDto> getPassword = await _passwordService.GetAllPasswordsForUserAsync(identity.id);
                return Ok(new ApiResponse(success: true, data: getPassword));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }


        [HttpPut("{passwordId}")] //to query
        public async Task<IActionResult> UpdatePassword(Guid passwordId, [FromBody] PasswordDto dataInput)
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                PasswordDto updatedPassword = await _passwordService.UpdatePassword(identity.id, passwordId, dataInput);
                return Ok(new ApiResponse(success: true, data: updatedPassword));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }


        [HttpDelete("{passwordId}")]
        public async Task<IActionResult> DeletePassword(Guid passwordId)
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                await _passwordService.DeletePasswordAsync(identity.id, passwordId);
                return Ok(new ApiResponse(success: true, data: "Password successfully deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }
    }
}
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
            if (password == null)
            {
                return BadRequest("Password data is null.");
            }

            var identity = await _passwordService.GetUserByTokenAsync(User);
            var userId = identity.id;
            var createdPassword = await _passwordService.CreateAsync(password, userId);

            return CreatedAtAction(nameof(PostPassword), new { id = createdPassword.passwordId }, createdPassword);
        }
    }
}
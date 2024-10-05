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
        public async Task<ActionResult<Password>> PostPassword([FromBody] Password password)
        {
            if (password == null)
            {
                return BadRequest("Password data is null.");
            }

            var createdPassword = await _passwordService.CreateAsync(password);
            return CreatedAtAction(nameof(PostPassword), new { id = createdPassword.passwordId }, createdPassword);
        }
    }
}
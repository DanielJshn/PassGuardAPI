using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private const int KEY_BYTES_LENGTH = 32;
        private const int VI_BYTES_LENGTH = 16;
        private readonly IAuthService _authService;
        private readonly IIdentityUser _identity;
        private readonly KeyConfig _keycon;
        private readonly EncryptionUtils _crypted;

        public AuthController(IAuthService authService, KeyConfig keycon, EncryptionUtils crypted, IIdentityUser identityUser)
        {
            _authService = authService;
            _keycon = keycon;
            _crypted = crypted;
            _identity = identityUser;
        }


        [AllowAnonymous]
        [HttpPost("Encrypted")] // this is for testing
        public IActionResult Encrypted(UserForRegistration userForRegistration)
        {

            string base64Key = _keycon.GetSecretKey();
            string base64IV = _keycon.GetIV();

            string encryptedEmail = _crypted.EncryptStringAES(userForRegistration.email, base64Key, base64IV);
            string encryptedPassword = _crypted.EncryptStringAES(userForRegistration.password, base64Key, base64IV);

            var result = new
            {
                EncryptedEmail = encryptedEmail,
                EncryptedPassword = encryptedPassword,
                Key = base64Key,
                IV = base64IV
            };
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegistration userForRegistration)
        {
            string token;
            string base64Key = _keycon.GetSecretKey();
            string base64IV = _keycon.GetIV();

            byte[] keyBytes = Convert.FromBase64String(base64Key);
            byte[] ivBytes = Convert.FromBase64String(base64IV);

            if (keyBytes.Length != KEY_BYTES_LENGTH) 
                throw new ArgumentException("Invalid key length. Must be 32 bytes for AES-256.");
            if (ivBytes.Length != VI_BYTES_LENGTH) 
                throw new ArgumentException("Invalid IV length. Must be 16 bytes for AES.");

            string decryptedEmail = _crypted.DecryptStringAES(userForRegistration.email, base64Key, base64IV);
            string decryptedPassword = _crypted.DecryptStringAES(userForRegistration.password, base64Key, base64IV);

            userForRegistration.email = decryptedEmail;
            userForRegistration.password = decryptedPassword;
            try
            {
                await _authService.ValidateRegistrationDataAsync(userForRegistration);
                await _authService.CheckUserExistsAsync(userForRegistration);
                token = await _authService.GenerateTokenAsync(userForRegistration);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
            return Ok(new ApiResponse(success: true, data: new { Token = token }));
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForRegistration userForLogin)
        {
            string newToken;
            try
            {
                string secretKey = _keycon.GetSecretKey();
                string secretIv = _keycon.GetIV();

                string decryptedEmail = _crypted.DecryptStringAES(userForLogin.email, secretKey, secretIv);
                string decryptedPassword = _crypted.DecryptStringAES(userForLogin.password, secretKey, secretIv);

                userForLogin.email = decryptedEmail;
                userForLogin.password = decryptedPassword;

                await _authService.CheckEmailAsync(userForLogin);
                await _authService.CheckPasswordAsync(userForLogin);
                newToken = await _authService.GenerateTokenForLogin(userForLogin);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
            return Ok(new ApiResponse(success: true, data: new { Token = newToken }));
        }

        [Authorize]
        [HttpDelete("deleteAllData")] 
        public async Task<IActionResult> DeletedAllData()
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                await _authService.DeleteAllDataByUserId(identity.id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Account deleted");
        }
    }
}
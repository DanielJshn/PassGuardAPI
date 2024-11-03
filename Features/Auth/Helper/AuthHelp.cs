using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apief;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace testProd.auth
{
    public class AuthHelp : IAuthHelp 
    {
        private readonly IConfiguration _config;
        private const int TOKEN_EXPIRATION_MONTHS = 1;
        private const string KEY_PASSWORD_KEY = "AppSettings:PasswordKey";
        public static string KEY_TOKEN_KEY = "AppSettings:TokenKey";

        public AuthHelp(IConfiguration config)
        {
            _config = config;
        }

        public string GetPasswordHash(string password)
        {
            string? passwordKeyString = _config.GetSection(KEY_PASSWORD_KEY).Value;
            if (string.IsNullOrEmpty(passwordKeyString))
            {
                throw new ArgumentException("PasswordKey is not configured.");
            }

            byte[] passwordKey = Encoding.ASCII.GetBytes(passwordKeyString);
          
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: passwordKey,  
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000, 
                numBytesRequested: 256 / 8
            );
            string passwordHashBase64 = Convert.ToBase64String(passwordHash);

            return passwordHashBase64;
        }

        public string GenerateNewToken(string userEmail)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, userEmail)
            };

            string? tokenKeyString = _config.GetSection(KEY_TOKEN_KEY).Value;

            if (string.IsNullOrEmpty(tokenKeyString))
            {
                throw new ArgumentException("TokenKey is not configured.");
            }

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString));
            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddMonths(TOKEN_EXPIRATION_MONTHS)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
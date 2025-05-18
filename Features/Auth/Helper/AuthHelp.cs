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
        public static string KEY_TOKEN_KEY = "JwtSettings:TokenKey";

        public AuthHelp(IConfiguration config)
        {
            _config = config;
        }

        public string GetPasswordHash(string password, string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("Salt is required.");

            byte[] saltBytes = Convert.FromBase64String(salt); // если соль присылается в base64
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );

            return Convert.ToBase64String(passwordHash);
        }



        public string GenerateNewToken(string userEmail)
        {

            Claim[] claims = new Claim[]
            {
                new Claim("email", userEmail)
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
                Expires = DateTime.Now.AddMonths(TOKEN_EXPIRATION_MONTHS),
                Issuer = _config["JwtSettings:ValidIssuer"],
                Audience = _config["JwtSettings:ValidAudience"]
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
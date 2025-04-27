using System.Security.Claims;
using testProd.auth;

namespace apief
{
    public class IdentityUser : IIdentityUser
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILog _logger;
        public IdentityUser(IAuthRepository authRepository, ILog log)
        {
            _authRepository = authRepository;
            _logger = log;
        }


        public async Task<UserData> GetUserByTokenAsync(ClaimsPrincipal userClaims)
        {
            _logger.LogInfo("Attempting to extract email from token claims...");
            
            var email = userClaims.FindFirstValue(ClaimTypes.Email) ?? userClaims.FindFirst("email")?.Value;
            Console.WriteLine("Extracted email: " + email);


            if (email == null)
            {
                foreach (var claim in userClaims.Claims)
                {
                    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
                }
                _logger.LogWarning("Email not found in token claims.");
                throw new UnauthorizedAccessException("User email not found in token.");
            }

            _logger.LogInfo("Extracted email: {Email}", email);

            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                throw new UnauthorizedAccessException("User not found.");
            }

            return user;
        }
    }
}
using System.Text.RegularExpressions;

namespace apief
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuthHelp _authHelp;
        private readonly ILog _logger;

        public AuthService(IAuthRepository authRepository, IAuthHelp authHelp, ILog logger)
        {
            _authRepository = authRepository;
            _authHelp = authHelp;
            _logger = logger;
        }

        private const int REQUIRED_USER_NAME_LENGTH = 3;
        private const int REQUIRED_USER_PASSWORD_LENGTH = 8;

        public async Task CheckUserExistsAsync(userForRegistration userForRegistration)
        {
            _logger.LogInfo("Checking if email exists for user: {Email}", userForRegistration.email);
            bool userExists = await CheckEmailExistsAsync(userForRegistration.email);

            if (userExists)
            {
                _logger.LogWarning("User with email {Email} already exists", userForRegistration.email);
                throw new Exception("User with this email already exists!");
            }
        }


        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            _logger.LogInfo("Checking if email exists: {Email}", email);
            var existingUser = await _authRepository.GetUserByEmailAsync(email);

            return existingUser != null;
        }







        public async Task CheckEmailAsync(userForRegistration userForLogin)
        {
            _logger.LogInfo("Checking if email exists for login: {Email}", userForLogin.email);
            bool userExists = await CheckEmailExistsAsync(userForLogin.email);

            if (!userExists)
            {
                _logger.LogWarning("Incorrect email: {Email}", userForLogin.email);
                throw new Exception("Incorrect Email");
            }
        }




        public Task ValidateRegistrationDataAsync(userForRegistration userForRegistration)
        {
            _logger.LogInfo("Validating registration data for user: {Email}", userForRegistration.email);

            if (userForRegistration.password.Length < REQUIRED_USER_PASSWORD_LENGTH ||
                !userForRegistration.password.Any(char.IsDigit) ||
                !userForRegistration.password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                _logger.LogWarning("Invalid password for user: {Email}", userForRegistration.email);
                throw new InvalidOperationException("Password must be at least 8 characters long, contain at least one digit and one special character.");
            }

            if (!IsEmailValid(userForRegistration.email))
            {
                _logger.LogWarning("Invalid email format: {Email}", userForRegistration.email);
                throw new InvalidOperationException("Email must contain '@' symbol.");
            }

            return Task.CompletedTask;
        }


        private bool IsEmailValid(string email)
        {
            _logger.LogInfo("Validating email format: {Email}", email);
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match match = regex.Match(email);
            return match.Success;
        }


        public async Task CheckPasswordAsync(userForRegistration userForLogin)
        {
            _logger.LogInfo("Checking password for user: {Email}", userForLogin.email);
            User? user = await _authRepository.GetUserByEmailAsync(userForLogin.email);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", userForLogin.email);
                throw new Exception("User not found");
            }

            string inputPasswordHash = _authHelp.GetPasswordHash(userForLogin.password);

            if (!inputPasswordHash.Equals(user.passwordHash))
            {
                _logger.LogWarning("Incorrect password for user: {Email}", userForLogin.email);
                throw new Exception("Incorrect Password");
            }
        }

        public async Task<string> GenerateTokenAsync(userForRegistration userForRegistration)
        {

            _logger.LogInfo("Generating token for user: {Email}", userForRegistration.email);
            string passwordHash2 = _authHelp.GetPasswordHash(userForRegistration.password);
            string token = _authHelp.GenerateNewToken(userForRegistration.email);

            var tokenEntity = new User
            {
                id = Guid.NewGuid(),
                email = userForRegistration.email,
                passwordHash = passwordHash2,
            };

            await _authRepository.AddUserAsync(tokenEntity);
            _logger.LogInfo("Token generated for user: {Email}", userForRegistration.email);

            return token;
        }


        public Task<string> GenerateTokenForLogin(userForRegistration userAuthDto)
        {
            _logger.LogInfo("Generating token for login: {Email}", userAuthDto.email);
            string email = userAuthDto.email;

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("User not found with this email", userAuthDto.email);
                throw new Exception("User not found");
            }

            string token = _authHelp.GenerateNewToken(email);
            _logger.LogInfo("Token generated for login: {Email}", email);
            return Task.FromResult(token);
        }

    }
}
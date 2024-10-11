using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;


namespace apief.Services
{
    public class PassService : IPassService
    {
        private readonly IPassRepository _passwordRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public PassService(IPassRepository passwordRepository, IAuthRepository authRepository, IMapper mapper, ILog logger)
        {
            _passwordRepository = passwordRepository;
            _mapper = mapper;
            _authRepository = authRepository;
            _logger = logger;
        }


        public async Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId)
        {
            _logger.LogInfo("Starting password creation for user with ID: {UserId}", userId);

            var passModel = _mapper.Map<Password>(passwordDto);

            passModel.passwordId = Guid.NewGuid();
            passModel.id = userId;

            _logger.LogInfo("Generated new password ID: {PasswordId}", passModel.passwordId);

            foreach (var additionalField in passModel.additionalFields)
            {
                additionalField.passwordId = passModel.passwordId;
                _logger.LogInfo("Assigned password ID: {PasswordId} to additional field: {Title}", passModel.passwordId, additionalField.title);
            }
            try
            {
                await _passwordRepository.AddAsync(passModel);
                _logger.LogInfo("Password with ID: {PasswordId} successfully added to the database", passModel.passwordId);
            }
            catch
            {
                _logger.LogWarning("Error occurred while adding password with ID: {PasswordId} to the database", passModel.passwordId);
                throw;
            }

            var createdPasswordDto = _mapper.Map<PasswordDto>(passModel);
            _logger.LogInfo("Password with ID: {PasswordId} successfully mapped to DTO");

            return createdPasswordDto;
        }


        public async Task<List<Password>> GetAllPasswordsForUserAsync(Guid userId)
        {
            _logger.LogInfo("Fetching all passwords for user with ID: {UserId}", userId);

            List<Password> passwords;

            try
            {
                passwords = await _passwordRepository.GetAllPasswordsByUserIdAsync(userId);
                _logger.LogInfo("Successfully fetched {PasswordCount} passwords for user with ID: {UserId}", passwords.Count, userId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error occurred while fetching passwords for user with ID: {UserId}. Exception: {ExceptionMessage}", userId, ex.Message);
                throw;
            }

            return passwords;
        }


        public async Task<User> GetUserByTokenAsync(ClaimsPrincipal userClaims)
        {
            _logger.LogInfo("Attempting to extract email from token claims...");

            var email = userClaims.FindFirstValue(ClaimTypes.Email) ?? userClaims.FindFirst("email")?.Value;

            if (email == null)
            {
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

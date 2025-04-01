using AutoMapper;

namespace apief
{
    public class AuthService : IAuthService
    {
        private readonly ILog _log;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IMapper mapper, ILog log)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _log = log;
        }

        public async Task<UserData> CreateNewAccountAsync(UserDataRegistrationDto userDto)
        {
            _log.LogInfo("Starting user account creation...");

            var validationErrors = Validate(userDto);
            if (validationErrors.Any())
            {
                throw new ArgumentException(string.Join(" ", validationErrors));
            }

            var userModel = _mapper.Map<UserData>(userDto);
            userModel.id = Guid.NewGuid();

            _log.LogInfo($"Generated new user ID: {userModel.id}");
            try
            {
                _log.LogInfo("Adding user to the database...");
                await _authRepository.AddUserDataAsync(userModel);
                _log.LogInfo("User successfully added to the database.");
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error while adding user to the database: {ex.Message}", ex);
                throw new Exception("An error occurred while saving the user data.");
            }

            return userModel;
        }


        private List<string> Validate(UserDataRegistrationDto userDto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(userDto.email))
                errors.Add("Email is required.");

            if (string.IsNullOrWhiteSpace(userDto.hashedPK))
                errors.Add("HashedPK is required.");

            if (string.IsNullOrWhiteSpace(userDto.hashedPKSalt))
                errors.Add("HashedPKSalt is required.");

            if (string.IsNullOrWhiteSpace(userDto.encryptedSK))
                errors.Add("EncryptedSK is required.");

            if (string.IsNullOrWhiteSpace(userDto.hashedRK))
                errors.Add("HashedRK is required.");

            if (string.IsNullOrWhiteSpace(userDto.recoverySK))
                errors.Add("RecoverySK is required.");

            return errors;
        }
    }
}
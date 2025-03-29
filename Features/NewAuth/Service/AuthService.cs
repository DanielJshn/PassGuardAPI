using AutoMapper;

namespace apief
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<UserData> CreateNewAccountAsync(UserDataRegistrationDto userDto)
        {
          var userModel = _mapper.Map<UserData>(userDto);
          userModel.id = Guid.NewGuid();
          await _authRepository.AddUserDataAsync(userModel);
          return userModel;
        }
    }
}
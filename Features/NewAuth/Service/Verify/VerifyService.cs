namespace apief
{
    public class VerifyService : IVerifyService
    {
        private readonly IAuthRepository _authRepository;
        public VerifyService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task SendOTP(string email, Guid id)
        {
             
        }
    }
}
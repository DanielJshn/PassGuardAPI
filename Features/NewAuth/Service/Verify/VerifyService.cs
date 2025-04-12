namespace apief
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository _verifyRepository;
        public VerifyService(IVerifyRepository verifyRepository)
        {
            _verifyRepository = verifyRepository;
        }

        public async Task SendOTP(string email, Guid id)
        {
            await SaveOTP(email, id);
        }
        
        private async Task SaveOTP(string email, Guid id)
        {
            var otp = CreateOTP();
            var otpData = new OTP
            {
                id = id,
                email = email,
                otpCode = otp,
                expirationDate = DateTime.UtcNow.AddMinutes(10)
            };
            await _verifyRepository.AddOTP(otpData);
        }

        private string CreateOTP()
        {
            var random = new Random();
            int code = random.Next(0, 1000000);
            string otpCode = code.ToString("D6");

            return otpCode;
        }

    }
}
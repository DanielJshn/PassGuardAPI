namespace apief
{
    public interface IVerifyRepository
    {
        Task AddOTP(OTP otp);
        Task<OTP?> GetOTPbyEmailAsync(string email);
    }
}
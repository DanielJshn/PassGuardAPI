namespace apief
{
    public interface IVerifyService
    {
        Task SendOTP(string email);
        Task CheckOTP(OTPdto otp);
    }
}
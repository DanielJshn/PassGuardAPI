namespace apief
{
    public interface IVerifyService
    {
        Task SendOTP(string email, Guid id);
    }
}
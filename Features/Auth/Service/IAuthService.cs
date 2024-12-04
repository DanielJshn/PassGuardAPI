namespace apief
{
    public interface IAuthService
    {
        Task CheckUserExistsAsync(UserForRegistration userForRegistration);
        Task CheckEmailAsync(UserForRegistration userForLogin);
        Task<string> GenerateTokenAsync(UserForRegistration userForRegistration);
        Task CheckPasswordAsync(UserForRegistration userForLogin);
        Task ValidateRegistrationDataAsync(UserForRegistration userForRegistration);
        Task<string> GenerateTokenForLogin(UserForRegistration userAuthDto);
        Task DeleteAllDataByUserId(Guid userId);
    }
}
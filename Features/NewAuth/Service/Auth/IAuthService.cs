namespace apief
{
    public interface IAuthService
    {
        Task<UserData> CreateNewAccountAsync(UserDataRegistrationDto userDto);
        
    }
}
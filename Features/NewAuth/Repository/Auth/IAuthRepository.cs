namespace apief
{
    public interface IAuthRepository
    {
         Task AddUserDataAsync(UserData user);
         Task<UserData?> GetUserByEmailAsync(string email);
         Task UpdateIsVerify(UserData userData);
         Task<string?> GetHashPKSaltAsync(string email);
         Task<string?> GetNonceAsync(string email);
    }
}
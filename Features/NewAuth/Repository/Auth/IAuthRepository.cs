namespace apief
{
    public interface IAuthRepository
    {
         Task AddUserDataAsync(UserData user);
         Task<UserData?> GetUserByEmailAsync(string email);
         Task UpdateIsVerify(UserData userData);
         Task<string?> GetHashPKSaltAsync(string email);
         Task UpdateNonceAsync(string nonce, string email);
    }
}
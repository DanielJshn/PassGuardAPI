namespace apief
{
    public interface IAuthRepository
    {
        Task<UserData?> GetUserByEmailAsync(string email);
        Task AddUserAsync(UserData user);
        Task DeleteData(Guid userId);
    }
}
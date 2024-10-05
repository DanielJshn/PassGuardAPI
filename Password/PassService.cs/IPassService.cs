namespace apief
{
    public interface IPassService
    {
        Task<List<Password>> GetAllPasswordsAsync(int userId);
        Task<Password> UpdatePasswordAsync(Guid id, Password userInput);
        Task<Password> PostPasswordAsync(int userId, Password passwordInput);
        Task DeletePasswordAsync(Guid id);
    }
}
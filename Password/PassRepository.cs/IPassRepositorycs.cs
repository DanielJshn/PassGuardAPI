namespace apief
{
    public interface IPassRepository
    {
        Task<List<Password>> GetAllPasswords(int userId);
        Task<Password> UpdatePassword(Guid id, Password userInput);
        Task<Password> PostPassword(int userId, Password passwordInput);
        Task DeletePassword(Guid id);
    }

}
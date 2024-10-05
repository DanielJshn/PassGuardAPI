
namespace apief
{
    public class PassService : IPassService
    {
        public Task DeletePasswordAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Password>> GetAllPasswordsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Password> PostPasswordAsync(int userId, Password passwordInput)
        {
            throw new NotImplementedException();
        }

        public Task<Password> UpdatePasswordAsync(Guid id, Password userInput)
        {
            throw new NotImplementedException();
        }
    }
}
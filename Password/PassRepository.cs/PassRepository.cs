
namespace apief
{
    public class PassRepository : IPassRepository
    {
        public Task DeletePassword(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Password>> GetAllPasswords(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Password> PostPassword(int userId, Password passwordInput)
        {
            throw new NotImplementedException();
        }

        public Task<Password> UpdatePassword(Guid id, Password userInput)
        {
            throw new NotImplementedException();
        }
    }
}
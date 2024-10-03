using apief;
using Microsoft.EntityFrameworkCore;

namespace testProd.auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }
    }
}
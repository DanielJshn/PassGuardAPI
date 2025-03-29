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

        public async Task<UserData> GetUserByEmailAsync(string email)
        {
            return await _dataContext.UserDatas.FirstOrDefaultAsync(u => u.email == email);
        }


        public async Task AddUserAsync(UserData user)
        {
            await _dataContext.UserDatas.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }


        public async Task DeleteData(Guid userId)
        {
            var user = await _dataContext.UserDatas
                 .FirstOrDefaultAsync(p => p.id == userId);
            if (user != null)
            {
                _dataContext.UserDatas.Remove(user);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
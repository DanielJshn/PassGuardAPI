using Microsoft.EntityFrameworkCore;

namespace apief
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUserDataAsync(UserData userData)
        {
            await _dataContext.UserDatas.AddAsync(userData);
            await _dataContext.SaveChangesAsync();
        }


        public async Task<UserData?> GetUserByEmailAsync(string email)
        {
            return await _dataContext.UserDatas.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task UpdateIsVerify(UserData userData)
        {
            _dataContext.UserDatas.Update(userData);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<string?> GetHashPKSaltAsync(string email)
        {
            var user = await _dataContext.UserDatas.FirstOrDefaultAsync(u => u.email == email);
            return user?.hashedPKSalt;
        }

        public async Task<string?> GetNonceAsync(string email)
        {
            var user = await _dataContext.UserDatas.FirstOrDefaultAsync(u => u.email == email);
            return user?.nonce;
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace apief
{
    public class VerifyRepository : IVerifyRepository
    {
        private readonly DataContext _dataContext;
        public VerifyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddOTP(OTP otp)
        {
            await _dataContext.OTPs.AddAsync(otp);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<OTP?> GetOTPbyEmailAsync(string email)
        {
            return await _dataContext.OTPs.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task UpdateOTP(OTP otp)
        {
            _dataContext.OTPs.Update(otp);
            await _dataContext.SaveChangesAsync();
        }

    }
}
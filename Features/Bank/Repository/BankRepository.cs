using Microsoft.EntityFrameworkCore;

namespace apief
{
    public class BankRepository : IBankRepository
    {
        private readonly DataContext _dataContext;
        public BankRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddBankDataAsync(BankAccount bankAccount)
        {
            await _dataContext.BankAccounts.AddAsync(bankAccount);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetBanksAsync(Guid userId)
        {

            return await _dataContext.BankAccounts
                .Where(t => t.id == userId)
                .ToListAsync();
        }
        public async Task<BankAccount> GetBankAccountByBankId(Guid bankId)
        {
            var result = await _dataContext.BankAccounts.FirstOrDefaultAsync(t => t.bankAccountId == bankId);
            
            if (result == null)
            {
                throw new Exception("Note is not found");
            }
            return result;
        }


        public async Task UpdateAsync(BankAccount bankAccount)
        {
            _dataContext.BankAccounts.Update(bankAccount);

            await _dataContext.SaveChangesAsync();
        }
    }
}
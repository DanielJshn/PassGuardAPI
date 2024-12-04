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

        public async Task AddBankAccountAsync(BankAccount bankAccount)
        {
            await _dataContext.BankAccounts.AddAsync(bankAccount);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync(Guid userId)
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


        public async Task UpdateBankAccountAsync(BankAccount bankAccount)
        {
            _dataContext.BankAccounts.Update(bankAccount);

            await _dataContext.SaveChangesAsync();
        }


        public async Task DeleteBankAccountAsync(Guid bankId)
        {
            var bank = await _dataContext.BankAccounts
                .FirstOrDefaultAsync(p => p.bankAccountId == bankId);

            if (bank != null)
            {
                _dataContext.BankAccounts.Remove(bank);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
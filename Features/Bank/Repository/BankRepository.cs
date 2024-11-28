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
    }
}
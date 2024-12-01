namespace apief
{
    public interface IBankRepository
    {
        Task AddBankDataAsync(BankAccount bankAccount);
        Task<IEnumerable<BankAccount>> GetBanksAsync(Guid userId);
        Task UpdateAsync(BankAccount bankAccount);
        Task<BankAccount> GetBankAccountByBankId(Guid bankId);
    }
}
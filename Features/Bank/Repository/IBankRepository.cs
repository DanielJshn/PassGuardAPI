namespace apief
{
    public interface IBankRepository
    {
        Task AddBankAccountAsync(BankAccount bankAccount);
        Task<IEnumerable<BankAccount>> GetBankAccountsAsync(Guid userId);
        Task<BankAccount> GetBankAccountByBankId(Guid bankId);
        Task UpdateBankAccountAsync(BankAccount bankAccount);
        Task DeleteBankAccountAsync(Guid noteId);
    }
}
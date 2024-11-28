namespace apief
{
    public interface IBankRepository
    {
        Task AddBankDataAsync(BankAccount bankAccount);
    }
}
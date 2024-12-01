namespace apief
{
    public interface IBankService
    {
        Task<BankAccountResponseDto> CreateBancAccountAsync(BankAccountDto bankAccountDto, Guid userId);
        Task<List<BankAccountResponseDto>> GetBankAccountsAsync(Guid userId);
        Task<BankAccountResponseDto> UpdateBankAccountAsync(Guid bankId, BankAccountUpdateDto bankAccountDto, Guid userId);
    }
}
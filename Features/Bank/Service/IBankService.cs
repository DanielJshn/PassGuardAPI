namespace apief
{
    public interface IBankService
    {
        Task<BankAccountResponseDto> CreateBancAccountAsync(BankAccountDto bankAccountDto, Guid userId);
    }
}
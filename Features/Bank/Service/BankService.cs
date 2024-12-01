using AutoMapper;

namespace apief
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        public BankService(IBankRepository bankRepository, IMapper mapper, ILog logger)
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BankAccountResponseDto> CreateBancAccountAsync(BankAccountDto bankAccountDto, Guid userId)
        {
            var bankModel = _mapper.Map<BankAccount>(bankAccountDto);
            bankModel.id = userId;
            bankModel.createdTime = DateTime.UtcNow.ToString();
            bankModel.modifiedTime = null;

            await _bankRepository.AddBankDataAsync(bankModel);

            return _mapper.Map<BankAccountResponseDto>(bankModel);
        }


        public async Task<List<BankAccountResponseDto>> GetBankAccountsAsync(Guid userId)
        {
            var banks = await _bankRepository.GetBanksAsync(userId);

            if (banks == null || !banks.Any())
            {
                return new List<BankAccountResponseDto>();
            }

            return _mapper.Map<List<BankAccountResponseDto>>(banks);
        }


        public async Task<BankAccountResponseDto> UpdateBankAccountAsync(Guid bankId, BankAccountUpdateDto bankAccountDto, Guid userId)
        {
           
            
            var bank = await _bankRepository.GetBankAccountByBankId(bankId);
            if (bank == null)
            {  
                throw new KeyNotFoundException($"Note with ID {bankId} not found.");
            }
           
            bank.cardHolderName = bankAccountDto.cardHolderName;
            bank.cardNumber = bankAccountDto.cardNumber;
            bank.cvv = bankAccountDto.cvv;
            bank.pin = bankAccountDto.pin;
            bank.expirationDateTimeStamp = bankAccountDto.expirationDateTimeStamp;
            bank.modifiedTime = DateTime.UtcNow.ToString();
            bank.categoryId = bankAccountDto.categoryId;
            
            await _bankRepository.UpdateAsync(bank);

            return _mapper.Map<BankAccountResponseDto>(bank);
        }
    }
}
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
    }
}
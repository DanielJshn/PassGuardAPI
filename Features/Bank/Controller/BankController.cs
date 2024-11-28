using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IIdentityUser _identity;
        private readonly IBankService _bankService;
        public BankController(IIdentityUser identity, IBankService bankService)
        {
            _identity = identity;
            _bankService = bankService;
        }


        [HttpPost]
        public async Task<IActionResult> PostBankData(BankAccountDto bankDto)
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                var createdBankAccount = await _bankService.CreateBancAccountAsync(bankDto, identity.id);
                return Ok(new ApiResponse(success: true, data: createdBankAccount));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }

    }
}
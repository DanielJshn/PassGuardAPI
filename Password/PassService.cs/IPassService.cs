using System.Security.Claims;

namespace apief
{
    public interface IPassService
    {
        Task<List<Password>> GetAllPasswordsForUserAsync(Guid userId);
        Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId);
        Task<User> GetUserByTokenAsync(ClaimsPrincipal userClaims);
    }
}
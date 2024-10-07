using System.Security.Claims;

namespace apief
{
    public interface IPassService
    {
        Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId);
        Task<User> GetUserByTokenAsync(ClaimsPrincipal userClaims);
    }
}
using System.Security.Claims;

namespace apief
{
    public interface IIdentityUser
    {
        Task<User> GetUserByTokenAsync(ClaimsPrincipal userClaims);
    }
}
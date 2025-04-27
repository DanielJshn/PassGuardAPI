using System.Security.Claims;

namespace apief
{
    public interface IIdentityUser
    {
        Task<UserData> GetUserByTokenAsync(ClaimsPrincipal userClaims);
    }
}
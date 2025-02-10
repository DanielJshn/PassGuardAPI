using System.Security.Claims;

namespace apief
{
    public interface IPassService
    {
        Task<List<PasswordResponsDto>> GetAllPasswordsForUserAsync(Guid userId);
        Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId );
        Task<PasswordDto> UpdatePassword(Guid id, Guid passwordId, PasswordForUpdateDto userInput);
        Task DeletePasswordAsync(Guid passwordId, Guid userId);
    }
}
namespace apief
{
    public interface IPassRepository
    {
        Task AddAsync(Password password);
        Task<List<Password>> GetAllPasswordsByUserIdAsync(Guid userId);
        Task<Password?> GetOnePasswordAsync(Guid userId, Guid passwordId);
        Task RemoveAdditionalFieldsAsync(List<AdditionalField> fieldsToRemove);
        Task AddAdditionalFieldAsync(AdditionalField field);
        Task DeletePasswordDataAsync(Guid passwordId);
    }
}
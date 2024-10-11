namespace apief
{
    public interface IPassRepository
    {
        Task AddAsync(Password password);
        Task<List<Password>> GetAllPasswordsByUserIdAsync(Guid userId);
        
    }
}
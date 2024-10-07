namespace apief
{
    public interface IPassRepository
    {
        Task AddAsync(Password password);
    }
}
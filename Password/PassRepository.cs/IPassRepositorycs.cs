namespace apief
{
    public interface IPassRepository
    {
        Task<Password> AddAsync(Password password);
    }
}
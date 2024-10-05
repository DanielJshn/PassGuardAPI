namespace apief
{
    public interface IPassService
    {
       Task<Password> CreateAsync(Password password);
    }
}
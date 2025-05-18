namespace apief
{
    public interface IAuthHelp 
    {
        public string GetPasswordHash(string password, string salt);
        public string GenerateNewToken(string userEmail);
    }
}
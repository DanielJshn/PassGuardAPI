namespace apief
{
    public class User
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string? email { get; set; }
        public string passwordHash { get; set; } = "";

    }
}
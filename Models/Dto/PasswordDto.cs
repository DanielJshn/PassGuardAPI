namespace apief
{
    public class PasswordDto
    {
        public Guid passwordId { get; set; } = Guid.NewGuid();
        public string? password { get; set; }
        public string? organization { get; set; }
        public string? title { get; set; }
        public string? lastEdit { get; set; } = null;
        public List<AdditionalFieldDto>? additionalFields { get; set; } = new List<AdditionalFieldDto>();
    }

    
}
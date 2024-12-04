namespace apief
{
    public class PasswordResponsDto
    {
        public Guid id { get; set; } 
        public Guid passwordId { get; set; } 
        public string? categoryId { get; set; }
        public string? password { get; set; }
        public string? organization { get; set; }
        public string? organizationLogo { get; set; }
        public string? title { get; set; }
        public string? createdTime { get; set; } = null;
        public string? modifiedTime { get; set; } = null;
        public List<AdditionalFieldDto> additionalFields { get; set; } = new List<AdditionalFieldDto>(); 
    }

}
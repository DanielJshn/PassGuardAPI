namespace apief
{
    public class Password
    {
        public Guid passwordId { get; set; }
        public Guid id { get; set; }
        public string? categoryId { get; set; }
        public string? password { get; set; }
        public string? organization { get; set; }
        public string? organizationLogo { get; set; }
        public string? title { get; set; }
        public string? createdTime { get; set; } = null;
        public string? modifiedTime { get; set; } = null;
        public List<AdditionalField> additionalFields { get; set; } = new List<AdditionalField>();
    }

}
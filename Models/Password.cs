namespace apief
{
    public class Password
    {
        public Guid? id { get; set; }= Guid.NewGuid();
        public Guid? passwordId { get; set; }= Guid.NewGuid();
        public string? password { get; set; }
        public string? organization { get; set; }
        public string? title { get; set; }
        public string? lastEdit { get; set; } = null;
        public List<AdditionalField>? additionalFields { get; set; } = new List<AdditionalField>();

        
    }
}
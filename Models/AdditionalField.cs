namespace apief
{
    public partial class AdditionalField
    {
        public Guid passwordId { get; set; } = Guid.NewGuid();
        public Guid additionalId { get; set; } = Guid.NewGuid();
        public string? title { get; set; }
        public string? value { get; set; }
        
    }
}
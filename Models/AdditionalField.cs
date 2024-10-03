namespace apief
{
    public partial class AdditionalField
    {
        public Guid passwordId { get; set; }= Guid.NewGuid();
        public Guid id { get; set; }= Guid.NewGuid();
        public string? title { get; set; }
        public string? value { get; set; }

        
    }
}
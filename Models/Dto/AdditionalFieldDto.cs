namespace apief
{
    public class AdditionalFieldDto
    {
        public Guid additionalId { get; set; } = Guid.NewGuid();
        public string? title { get; set; }
        public string? value { get; set; }
    }
}
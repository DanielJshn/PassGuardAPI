namespace apief
{
    public class Note
    {
        public Guid id { get; set; }= Guid.NewGuid();
        public Guid noteId { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? lastEdit { get; set; }
    }
}
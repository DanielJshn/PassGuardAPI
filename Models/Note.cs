namespace apief
{
    public class Note
    {
        public Guid? id { get; set; }= Guid.NewGuid();
        public Guid? noteId { get; set; }= Guid.NewGuid();
        public string? title { get; set; }
        public string? description { get; set; }
        public string? lastEdit { get; set; }
    }
}
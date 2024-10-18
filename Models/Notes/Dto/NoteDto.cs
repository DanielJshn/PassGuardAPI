namespace apief
{
    public class NoteDto
    {
        
        public Guid noteId { get; set; }= Guid.NewGuid();
        public string? title { get; set; }
        public string? description { get; set; }
        public string? lastEdit { get; set; }
    }
}
namespace apief
{
    public class NoteResponseDto
    {
        public Guid noteId { get; set; } 
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? lastEdit { get; set; }
    }
}
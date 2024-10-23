namespace apief
{
    public class NoteResponseDto
    {
        public Guid noteId { get; set; } = Guid.NewGuid();
        public string? title { get; set; }
        public string? description { get; set; }
        public DateTime? lastEdit { get; set; }
    }
}
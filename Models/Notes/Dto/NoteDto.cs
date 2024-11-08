namespace apief
{
    public class NoteDto
    {
        public Guid noteId { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? backgroundColorHex { get; set; }
        public string? categoryId  { get; set; }
    }
}
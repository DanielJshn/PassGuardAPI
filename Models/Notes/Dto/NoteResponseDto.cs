namespace apief
{
    public class NoteResponseDto
    {
        public Guid noteId { get; set; } 
        public string? title { get; set; }
        public string? description { get; set; }
        public string? backgroundColorHex { get; set; }
        public string? createdTime  { get; set; }
        public string? modifiedTime  { get; set; }
        public string? categoryId  { get; set; }
    }
}
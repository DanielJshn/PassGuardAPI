namespace apief
{
    public class Note
    {
        public Guid id { get; set; }= Guid.NewGuid();
        public Guid noteId { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? backgroundColorHex { get; set; }
        public string? createdTime  { get; set; }
        public string? modifiedTime  { get; set; }
        public string? categoryId  { get; set; }
    }
}
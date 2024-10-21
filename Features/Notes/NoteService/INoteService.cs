namespace apief
{
    public interface INoteService
    {
        Task<NoteDto> CreateNoteAsync(NoteDto noteDto, Guid userId);
        Task<List<NoteDto>> GetNotesAsync(Guid userId);
    }
}
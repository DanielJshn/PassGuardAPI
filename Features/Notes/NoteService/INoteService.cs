namespace apief
{
    public interface INoteService
    {
        Task<NoteDto> CreateNoteAsync(NoteDto noteDto, Guid userId);
    }
}
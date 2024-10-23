namespace apief
{
    public interface INoteService
    {
        Task<NoteResponseDto> CreateNoteAsync(NoteDto noteDto, Guid userId);
        Task<List<NoteResponseDto>> GetNotesAsync(Guid userId);
        Task<NoteResponseDto> UpdateNoteAsync(Guid noteid, NoteDto noteDto, Guid userId);
    }
}
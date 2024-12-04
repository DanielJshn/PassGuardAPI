namespace apief
{
    public interface INoteService
    {
        Task<NoteResponseDto> CreateNoteAsync(NoteDto noteDto, Guid userId);
        Task<List<NoteResponseDto>> GetNotesAsync(Guid userId);
        Task<NoteResponseDto> UpdateNoteAsync(Guid noteId, NoteUpdateDto noteDto, Guid userId);
        Task DeleteNoteAsync(Guid noteId, Guid userId);
    }
}
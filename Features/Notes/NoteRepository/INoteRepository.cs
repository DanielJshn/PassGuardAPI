namespace apief
{
    public interface INoteRepository
    {
        Task AddAsync(Note note);
        Task<IEnumerable<Note>> GetNotesAsync(Guid userId);
    }
}
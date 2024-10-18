namespace apief
{
    public interface INoteRepository
    {
        Task AddAsync(Note note);
    }
}
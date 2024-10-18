namespace apief
{
    public class NoteRepository : INoteRepository
    {
        private readonly DataContext _dataContext;

        public NoteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task AddAsync(Note note)
        {
            await _dataContext.Notes.AddAsync(note);
            await _dataContext.SaveChangesAsync();
        }
    }
}
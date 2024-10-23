using Microsoft.EntityFrameworkCore;

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


        public async Task<IEnumerable<Note>> GetNotesAsync(Guid userId)
        {

            return await _dataContext.Notes
                .Where(t => t.id == userId)
                .ToListAsync();
        }


        public async Task<Note> GetNoteByUserId(Guid noteId)
        {
            return await _dataContext.Notes.FirstOrDefaultAsync(t => t.noteId == noteId);
        }


        public async Task UpdateAsync(Note note)
        {
            _dataContext.Notes.Update(note);

            await _dataContext.SaveChangesAsync();
        }
    }
}
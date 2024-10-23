using System.Collections.Immutable;
using AutoMapper;

namespace apief
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public NoteService(INoteRepository noteRepository, IMapper mapper, ILog logger)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<NoteResponseDto> CreateNoteAsync(NoteDto noteDto, Guid userId)
        {

            if (string.IsNullOrWhiteSpace(noteDto.title))
            {
                throw new Exception("Title is required"); ;
            }
     

            if (string.IsNullOrEmpty(noteDto.title))
            {
           
                throw new ArgumentException("Title is required");
            }

            var noteModel = _mapper.Map<Note>(noteDto);
            noteModel.noteId = Guid.NewGuid();
            noteModel.id = userId;
            noteModel.lastEdit = DateTime.UtcNow;


            await _noteRepository.AddAsync(noteModel);

            
            return _mapper.Map<NoteResponseDto>(noteModel);
        }


        public async Task<List<NoteResponseDto>> GetNotesAsync(Guid userId)
        {
            var notes = await _noteRepository.GetNotesAsync(userId);

            _logger.LogInfo("Tasks for user {UserId} retrieved successfully.", userId);

            return _mapper.Map<List<NoteResponseDto>>(notes);
        }


        public async Task<NoteResponseDto> UpdateNoteAsync(Guid noteid, NoteDto noteDto, Guid userId)
        {
            var note = await _noteRepository.GetNoteByUserId(noteid);

            note.title = noteDto.title;
            note.description = noteDto.description;
            note.lastEdit = DateTime.UtcNow;
            note.noteId= noteid;

            await _noteRepository.UpdateAsync(note);

            _logger.LogInfo("Task {TaskId} updated successfully for user {UserId}.", noteid, userId);
            return _mapper.Map<NoteResponseDto>(note);
        }
    }
}
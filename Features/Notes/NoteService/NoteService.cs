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


        public async Task<NoteDto> CreateNoteAsync(NoteDto noteDto, Guid userId)
        {

            if (string.IsNullOrWhiteSpace(noteDto.title))
            {
                throw new Exception("Title is required"); ;
            }
            _logger.LogInfo("Creating new task for user {UserId}.", userId);

            if (string.IsNullOrEmpty(noteDto.title))
            {
                _logger.LogWarning("Task creation failed: Title is required.");
                throw new ArgumentException("Title is required");
            }

            var noteModel = _mapper.Map<Note>(noteDto);
            noteModel.noteId = Guid.NewGuid();
            noteModel.id = userId;
            noteModel.lastEdit = DateTime.UtcNow.ToString();
            

            await _noteRepository.AddAsync(noteModel);

            _logger.LogInfo("Task {TaskId} created successfully for user {UserId}.", noteModel.id, userId);
            return _mapper.Map<NoteDto>(noteDto);
        }
    }
}
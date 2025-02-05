using System.Collections.Immutable;
using AutoMapper;

namespace apief
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        private readonly ICacheService _cacheService;

        public NoteService(INoteRepository noteRepository, IMapper mapper, ILog logger, ICacheService cacheService)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }


        public async Task<NoteResponseDto> CreateNoteAsync(NoteDto noteDto, Guid userId)
        {
            _logger.LogInfo("Start creating a note for user {UserId}.", userId);

            if (string.IsNullOrWhiteSpace(noteDto.title))
            {
                _logger.LogWarning("Failed to create a note for user {UserId}: Title is required.", userId);
                throw new ArgumentException("Title is required");
            }

            var noteModel = _mapper.Map<Note>(noteDto);
            noteModel.id = userId;
            noteModel.createdTime = DateTime.UtcNow.ToString();
            noteModel.modifiedTime = null;

            _logger.LogInfo("Creating note {NoteId} for user {UserId}.", noteModel.noteId, userId);

            await _noteRepository.AddAsync(noteModel);

            await _cacheService.RemoveAsync($"notes_{userId}");

            _logger.LogInfo("Note {NoteId} created successfully for user {UserId}.", noteModel.noteId, userId);

            return _mapper.Map<NoteResponseDto>(noteModel);
        }


        public async Task<List<NoteResponseDto>> GetNotesAsync(Guid userId)
        {
            _logger.LogInfo("Start retrieving notes for user {UserId}.", userId);

            string cacheKey = $"notes_{userId}";

            var cachedNotes = await _cacheService.GetAsync<List<NoteResponseDto>>(cacheKey);
            if (cachedNotes != null)
            {
                _logger.LogInfo("Returning cached notes for user {UserId}.", userId);
                return cachedNotes;
            }

            var notes = await _noteRepository.GetNotesAsync(userId);

            if (notes == null || !notes.Any())
            {
                _logger.LogWarning("No notes found for user {UserId}.", userId);
                return new List<NoteResponseDto>();
            }

            var noteDtos = _mapper.Map<List<NoteResponseDto>>(notes);

            await _cacheService.SetAsync(cacheKey, noteDtos, TimeSpan.FromMinutes(10));

            _logger.LogInfo("{NoteCount} notes retrieved successfully for user {UserId}.", noteDtos.Count, userId);

            return noteDtos;
        }


        public async Task<NoteResponseDto> UpdateNoteAsync(Guid noteId, NoteUpdateDto noteDto, Guid userId)
        {
            _logger.LogInfo("Start updating note {NoteId} for user {UserId}.", noteId, userId);

            var note = await _noteRepository.GetNoteByNoteId(noteId);
            if (note == null)
            {
                _logger.LogWarning("Note {NoteId} not found for user {UserId}.", noteId, userId);
                throw new KeyNotFoundException($"Note with ID {noteId} not found.");
            }

            if (string.IsNullOrWhiteSpace(noteDto.title))
            {
                _logger.LogWarning("Failed to update note {NoteId} for user {UserId}: Title is required.", noteId, userId);
                throw new ArgumentException("Title is required.");
            }

            note.title = noteDto.title;
            note.description = noteDto.description;
            note.backgroundColorHex = noteDto.backgroundColorHex;
            note.modifiedTime = DateTime.UtcNow.ToString();
            note.categoryId = noteDto.categoryId;

            _logger.LogInfo("Updating note {NoteId} for user {UserId}.", note.noteId, userId);

            await _noteRepository.UpdateAsync(note);

            _logger.LogInfo("Note {NoteId} updated successfully for user {UserId}.", note.noteId, userId);

            return _mapper.Map<NoteResponseDto>(note);
        }


        public async Task DeleteNoteAsync(Guid noteId, Guid userId)
        {
            _logger.LogInfo("Start deleting note with ID: {NoteId} for user: {UserId}.", noteId, userId);

            var existingNote = await _noteRepository.GetNoteByNoteId(noteId);

            if (existingNote == null)
            {
                _logger.LogWarning("Note with ID: {NoteId} not found for user: {UserId}.", noteId, userId);
                throw new Exception("Note not found");
            }

            await _noteRepository.DeleteNoteAsync(noteId);

            _logger.LogInfo("Note with ID: {NoteId} successfully deleted for user: {UserId}.", noteId, userId);
        }
    }
}
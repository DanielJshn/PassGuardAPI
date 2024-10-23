using Microsoft.AspNetCore.Mvc;

namespace apief
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _notesService;
        private readonly IIdentityUser _identity;

        public NoteController(INoteService noteService, IIdentityUser identityUser)
        {
            _notesService = noteService;
            _identity = identityUser;
        }


        [HttpPost]
        public async Task<IActionResult> PostNote(NoteDto noteDto)
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                var createdNote = await _notesService.CreateNoteAsync(noteDto, identity.id);
                return Ok(new ApiResponse(success: true, data: createdNote));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNote()
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                var response = await _notesService.GetNotesAsync(identity.id);
                return Ok(new ApiResponse(success: true, data: response));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }


        [HttpPut("id")]
        public async Task<IActionResult> PutNote(Guid noteid, NoteDto note)
        {
            try
            {
                var identity = await _identity.GetUserByTokenAsync(User);
                var updateNote = await _notesService.UpdateNoteAsync(noteid, note, identity.id);
                return Ok(new ApiResponse(success: true, data: updateNote));

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(success: false, message: ex.Message));
            }
        }
    }
}
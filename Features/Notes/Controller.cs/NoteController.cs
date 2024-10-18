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
    }
}
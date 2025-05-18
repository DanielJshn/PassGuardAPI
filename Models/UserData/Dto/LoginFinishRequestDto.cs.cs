using System.ComponentModel.DataAnnotations;

namespace apief
{
    public class LoginFinishRequestDto
    {
        public string? email { get; set; }
        public string? hashedPK { get; set; }
    }
}
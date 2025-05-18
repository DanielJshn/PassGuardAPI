using System.ComponentModel.DataAnnotations;

namespace apief
{
    public class LoginFinishResponseDto
    {
        public Guid? id { get; set; }
        public string? accesToken { get; set; }
        public string? refreshToken { get; set; }
        public string? encryptedSK { get; set; }
        public string? token {get; set;}
    }
}
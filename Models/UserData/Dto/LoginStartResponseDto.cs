using System.ComponentModel.DataAnnotations;

namespace apief
{
    public class LoginStartResponseDto
    {
        public string? hashedPKSalt { get; set; }
        public string? nonce { get; set; }
    }
}
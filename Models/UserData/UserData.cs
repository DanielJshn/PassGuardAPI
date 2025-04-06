using System.ComponentModel.DataAnnotations;

namespace apief
{
    public class UserData
    {
        public Guid id { get; set; }
        public string? email { get; set; }
        public string? hashedPK { get; set; }
        public string? hashedPKSalt { get; set; }
        public string? encryptedSK { get; set; }
        public string? hashedRK { get; set; }
        public string? recoverySK { get; set; }
        public bool isVerify { get; set; } = false;
    }
}
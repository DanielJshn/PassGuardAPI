namespace apief
{
    public class UserDataRegistrationDto
    {
        public string? email { get; set; }
        public string? hashedPK { get; set; }
        public string? hashedPKSalt { get; set; }
        public string? encryptedSK { get; set; }
        public string? hashedRK { get; set; }
        public string? recoverySK { get; set; }

    }
}
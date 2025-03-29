namespace apief
{
     public class UserDataRegistrationDto
    {
        public required string email { get; set; }
        public required string hashedPK { get; set; }
        public required string hashedPKSalt { get; set; }
        public required string encryptedSK { get; set; }
        public required string hashedRK { get; set; }
        public required string recoverySK { get; set; }
    
    }
}
namespace apief
{
    public class OTP
    {
        public Guid id { get; set; }
        public string? email { get; set; }
        public string? otpCode { get; set; }
        public DateTime expirationDate { get; set; }
    }
}
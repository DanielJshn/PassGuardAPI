namespace apief
{
    public class OTP
    {
        public string? email { get; set; }
        public string? otpCode { get; set; }
        public DateTime expirationDate { get; set; }
    }
}
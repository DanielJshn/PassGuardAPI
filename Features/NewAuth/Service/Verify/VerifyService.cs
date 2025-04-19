using System.Net;
using System.Net.Mail;

namespace apief
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository _verifyRepository;
        public VerifyService(IVerifyRepository verifyRepository)
        {
            _verifyRepository = verifyRepository;
        }

        public async Task SendOTP(string email, Guid id)
        {
            var otp = CreateOTP();
            await SaveOTP(email, id, otp);
            await SendEmailAsync(email, otp);
        }

        private string CreateOTP()
        {
            var random = new Random();
            int code = random.Next(0, 1000000);
            return code.ToString("D6");
        }

        private async Task SaveOTP(string email, Guid id, string otp)
        {
            var otpData = new OTP
            {
                id = id,
                email = email,
                otpCode = otp,
                expirationDate = DateTime.UtcNow.AddMinutes(10)
            };
            await _verifyRepository.AddOTP(otpData);
        }

        private async Task SendEmailAsync(string email, string otp)
        {
            var fromAddress = new MailAddress("danieldobosh361@gmail.com", " PassGuard");
            var toAddress = new MailAddress(email);
            const string fromPassword = "brzz wtej tobx hqol"; 
            const string subject = "Your OTP Code";
            string body = $"Your OTP code is: {otp}\nThis code will expire in 10 minutes.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            await smtp.SendMailAsync(message);
        }

    }
}
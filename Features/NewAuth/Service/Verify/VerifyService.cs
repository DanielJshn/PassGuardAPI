using System.Net;
using System.Net.Mail;
using AutoMapper;

namespace apief
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository _verifyRepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;


        public VerifyService(IVerifyRepository verifyRepository, IMapper mapper, IAuthRepository authRepository)
        {
            _verifyRepository = verifyRepository;
            _mapper = mapper;
            _authRepository = authRepository;

        }

        public async Task SendOTP(string email)
        {
            var otp = CreateOTP();
            await SaveOTP(email, otp);
            await SendEmailAsync(email, otp);
        }

        public async Task ResendOTP(string email)
        {
            var otp = CreateOTP();
            await UpdateOTP(email, otp);
            await SendEmailAsync(email, otp);
        }

        public async Task CheckOTP(OTPdto otp)
        {
            var otpFromDb = await _verifyRepository.GetOTPbyEmailAsync(otp.email);

            if (otpFromDb == null)
            {
                throw new Exception("OTP not found for the provided email.");
            }

            if (otpFromDb.otpCode != otp.otpCode)
            {
                throw new Exception("Invalid OTP code.");
            }

            if (otpFromDb.expirationDate < DateTime.UtcNow)
            {
                throw new Exception("OTP code has expired.");
            }

            var user = await _authRepository.GetUserByEmailAsync(otp.email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.isVerify = true;
            await _authRepository.UpdateIsVerify(user);
        }



        private string CreateOTP()
        {
            var random = new Random();
            int code = random.Next(0, 1000000);
            return code.ToString("D6");
        }

        private async Task SaveOTP(string email, string otp)
        {
            var otpData = new OTP
            {
                email = email,
                otpCode = otp,
                expirationDate = DateTime.UtcNow.AddMinutes(10)
            };
            await _verifyRepository.AddOTP(otpData);
        }

        private async Task UpdateOTP(string email, string otp)
        {
            var existingOtp = await _verifyRepository.GetOTPbyEmailAsync(email);

            if (existingOtp != null)
            {
                existingOtp.otpCode = otp;
                existingOtp.expirationDate = DateTime.UtcNow.AddMinutes(10);
                await _verifyRepository.UpdateOTP(existingOtp);
            }
            else
            {
                await SaveOTP(email, otp);
            }
        }


        private async Task SendEmailAsync(string email, string otp)
        {
            var fromAddress = new MailAddress("@gmail.com", " PassGuard");
            var toAddress = new MailAddress(email);
            const string fromPassword = ""; //Todo add your pass
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
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


        public async Task CheckOTP(OTPdto otp)
        {
            var otpFromEmail = _mapper.Map<OTP>(otp);
            otpFromEmail.expirationDate = DateTime.UtcNow;

            OTP otpFromDb = new OTP();

            otpFromDb = await _verifyRepository.GetOTPbyEmailAsync(otp.email);
            if (otpFromDb.otpCode != otp.otpCode)
            {
                throw new Exception();
            }
            if (otpFromDb.expirationDate < otpFromEmail.expirationDate)
            {
                throw new Exception();
            }
            UserData user = new UserData();
            user = await _authRepository.GetUserByEmailAsync(otpFromEmail.email);
            user.isVerify = true;
            _authRepository.UpdateIsVerify(user);


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
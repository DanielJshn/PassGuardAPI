using System.Net;
using System.Net.Mail;
using AutoMapper;

namespace apief
{
    public class VerifyService : IVerifyService
    {
        private readonly IVerifyRepository _verifyRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ILog _log;


        public VerifyService(IVerifyRepository verifyRepository, IAuthRepository authRepository, ILog log)
        {
            _verifyRepository = verifyRepository;
            _authRepository = authRepository;
            _log = log;

        }

        public async Task SendOTP(string email)
        {
            _log.LogInfo($"Starting to send new OTP to email: {email}");

            var otp = CreateOTP();

            try
            {
                _log.LogInfo("Saving new OTP to the database...");
                await SaveOTP(email, otp);
                _log.LogInfo("OTP successfully saved to the database.");

                _log.LogInfo("Sending OTP email...");
                await SendEmailAsync(email, otp);
                _log.LogInfo("OTP email successfully sent.");
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error occurred while sending OTP to {email}: {ex.Message}", ex);
                throw new Exception("An error occurred while sending the OTP.");
            }
        }

        public async Task ResendOTP(string email)
        {
            _log.LogInfo($"Starting to resend OTP to email: {email}");

            var otp = CreateOTP();

            try
            {
                _log.LogInfo("Updating OTP in the database...");
                await UpdateOTP(email, otp);
                _log.LogInfo("OTP successfully updated in the database.");

                _log.LogInfo("Sending OTP email...");
                await SendEmailAsync(email, otp);
                _log.LogInfo("OTP email successfully sent.");
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error occurred while resending OTP to {email}: {ex.Message}", ex);
                throw new Exception("An error occurred while resending the OTP.");
            }
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
            _log.LogInfo("Starting OTP code generation...");

            var random = new Random();
            int code = random.Next(0, 1000000);
            string otp = code.ToString("D6");

            _log.LogInfo($"Generated OTP code: {otp}");

            return otp;
        }

        private async Task SaveOTP(string email, string otp)
        {
            _log.LogInfo($"Starting to save OTP for email: {email}");

            var otpData = new OTP
            {
                email = email,
                otpCode = otp,
                expirationDate = DateTime.UtcNow.AddMinutes(10)
            };

            try
            {
                _log.LogInfo("Adding OTP data to the database...");
                await _verifyRepository.AddOTP(otpData);
                _log.LogInfo("OTP successfully saved to the database.");
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error while saving OTP for {email}: {ex.Message}", ex);
                throw new Exception("An error occurred while saving the OTP.");
            }
        }


        private async Task UpdateOTP(string email, string otp)
        {
            _log.LogInfo($"Starting to update OTP for email: {email}");

            try
            {
                var existingOtp = await _verifyRepository.GetOTPbyEmailAsync(email);

                if (existingOtp != null)
                {
                    _log.LogInfo($"Existing OTP found for email: {email}. Updating OTP and expiration date.");

                    existingOtp.otpCode = otp;
                    existingOtp.expirationDate = DateTime.UtcNow.AddMinutes(10);

                    await _verifyRepository.UpdateOTP(existingOtp);

                    _log.LogInfo($"OTP successfully updated for email: {email}");
                }
                else
                {
                    _log.LogInfo($"No existing OTP found for email: {email}. Saving new OTP.");
                    await SaveOTP(email, otp);
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Error while updating OTP for {email}: {ex.Message}", ex);
                throw new Exception("An error occurred while updating the OTP.");
            }
        }

        private async Task SendEmailAsync(string email, string otp)
        {
            _log.LogInfo($"Starting to send OTP email to: {email}");

            var fromAddress = new MailAddress("@gmail.com", "PassGuard");
            var toAddress = new MailAddress(email);
            const string fromPassword = ""; // TODO: Add your password
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

            try
            {
                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

                await smtp.SendMailAsync(message);

                _log.LogInfo($"OTP email successfully sent to: {email}");
            }
            catch (Exception ex)
            {
                _log.LogWarning($"Failed to send OTP email to {email}: {ex.Message}", ex);
                throw new Exception("An error occurred while sending the OTP email.");
            }
        }
    }
}
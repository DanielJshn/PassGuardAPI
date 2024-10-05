using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace apief.Services
{
    public class PassService : IPassService
    {
        private readonly IPassRepository _passwordRepository;

        public PassService(IPassRepository passwordRepository)
        {
            _passwordRepository = passwordRepository;
        }

         public async Task<Password> CreateAsync(Password password)
        {
            foreach (var additionalField in password.additionalFields)
        {
            additionalField.passwordId = password.passwordId; // Устанавливаем ID пароля
        }
            return await _passwordRepository.AddAsync(password);
        }

       
    }
}

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
            return await _passwordRepository.AddAsync(password);
        }

       
    }
}

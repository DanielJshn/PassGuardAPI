using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;


namespace apief.Services
{
    public class PassService : IPassService
    {
        private readonly IPassRepository _passwordRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public PassService(IPassRepository passwordRepository,IAuthRepository authRepository, IMapper mapper)
        {
            _passwordRepository = passwordRepository;
            _mapper = mapper;
            _authRepository = authRepository;
        }

        public async Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId)
        {
            var passModel = _mapper.Map<Password>(passwordDto);

            passModel.passwordId = Guid.NewGuid(); 
            passModel.id = userId;
            passModel.lastEdit = passwordDto.lastEdit;
            passModel.organization = passwordDto.organization;
            passModel.password = passwordDto.password;

            foreach (var additionalField in passModel.additionalFields)
            {
                additionalField.passwordId = passModel.passwordId;
            }

            await _passwordRepository.AddAsync(passModel);

            var createdPasswordDto = _mapper.Map<PasswordDto>(passModel);
            
            return createdPasswordDto;
        }

        

        public async Task<User> GetUserByTokenAsync(ClaimsPrincipal userClaims)
        {

            var email = userClaims.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                throw new UnauthorizedAccessException("User email not found in token.");
            }

            var user = await _authRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            return user;
        }


    }
}

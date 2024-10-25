using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace apief.Services
{
    public class PassService : IPassService
    {
        private readonly IPassRepository _passwordRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public PassService(IPassRepository passwordRepository, IMapper mapper, ILog logger)
        {
            _passwordRepository = passwordRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<PasswordDto> CreateAsync(PasswordDto passwordDto, Guid userId)
        {
            _logger.LogInfo("Starting password creation for user with ID: {UserId}", userId);

            var passModel = _mapper.Map<Password>(passwordDto);

            passModel.passwordId = Guid.NewGuid();
            passModel.id = userId;

            _logger.LogInfo("Generated new password ID: {PasswordId}", passModel.passwordId);

            foreach (var additionalField in passModel.additionalFields)
            {
                additionalField.passwordId = passModel.passwordId;
                _logger.LogInfo("Assigned password ID: {PasswordId} to additional field: {Title}", passModel.passwordId, additionalField.title);
            }
            try
            {
                await _passwordRepository.AddAsync(passModel);
                _logger.LogInfo("Password with ID: {PasswordId} successfully added to the database", passModel.passwordId);
            }
            catch
            {
                _logger.LogWarning("Error occurred while adding password with ID: {PasswordId} to the database", passModel.passwordId);
                throw;
            }

            var createdPasswordDto = _mapper.Map<PasswordDto>(passModel);
            _logger.LogInfo("Password with ID: {PasswordId} successfully mapped to DTO");

            return createdPasswordDto;
        }


        public async Task<List<PasswordResponsDto>> GetAllPasswordsForUserAsync(Guid userId)
        {
            _logger.LogInfo("Fetching all passwords for user with ID: {UserId}", userId);

            List<Password> passwords;

            try
            {
                passwords = await _passwordRepository.GetAllPasswordsByUserIdAsync(userId);
                _logger.LogInfo("Successfully fetched {PasswordCount} passwords for user with ID: {UserId}", passwords.Count, userId);

            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error occurred while fetching passwords for user with ID: {UserId}. Exception: {ExceptionMessage}", userId, ex.Message);
                throw;
            }
            var passwordDtos = passwords.Select(p => new PasswordResponsDto
            {
                id = p.id,
                passwordId = p.passwordId,
                password = p.password,
                organization = p.organization,
                title = p.title,
                lastEdit = p.lastEdit,
                additionalFields = p.additionalFields.Select(af => new AdditionalFieldDto
                {
                    title = af.title,
                    value = af.value
                }).ToList()
            }).ToList();

            return passwordDtos;
        }


        public async Task<PasswordDto> UpdatePassword(Guid userId, Guid passwordId, PasswordDto userInput)
        {
            _logger.LogInfo("Attempting to update password with ID: {PasswordId} for user: {UserId}", passwordId, userId);

            var existingPassword = await _passwordRepository.GetOnePasswordAsync(userId, passwordId);

            if (existingPassword == null)
            {
                _logger.LogWarning("Password with ID: {PasswordId} not found for user: {UserId}", passwordId, userId);
                throw new Exception("Password not found");
            }

            _logger.LogInfo("Password found for ID: {PasswordId}. Updating fields.", passwordId);

            existingPassword.password = userInput.password;
            existingPassword.organization = userInput.organization;
            existingPassword.title = userInput.title;
            existingPassword.lastEdit = userInput.lastEdit;

            var existingAdditionalFields = existingPassword.additionalFields.ToList();

            var fieldsToRemove = existingAdditionalFields
                .Where(f => !userInput.additionalFields.Any(dto => dto.additionalId == f.additionalId))
                .ToList();

            if (fieldsToRemove.Any())
            {
                _logger.LogInfo("Removing {FieldCount} additional fields from password with ID: {PasswordId}", fieldsToRemove.Count, passwordId);
                await _passwordRepository.RemoveAdditionalFieldsAsync(fieldsToRemove);
            }

            foreach (var fieldDto in userInput.additionalFields)
            {
                var existingField = existingAdditionalFields
                    .FirstOrDefault(f => f.additionalId == fieldDto.additionalId);

                if (existingField != null)
                {
                    _logger.LogInfo("Updating additional field with ID: {FieldId} for password ID: {PasswordId}", fieldDto.additionalId, passwordId);
                    existingField.title = fieldDto.title;
                    existingField.value = fieldDto.value;
                }
                else
                {
                    _logger.LogInfo("Adding new additional field for password ID: {PasswordId}", passwordId);
                    var newField = new AdditionalField
                    {
                        passwordId = existingPassword.passwordId,
                        additionalId = Guid.NewGuid(),
                        title = fieldDto.title,
                        value = fieldDto.value
                    };

                    await _passwordRepository.AddAdditionalFieldAsync(newField);
                }
            }

            _logger.LogInfo("Saving changes to the database for password ID: {PasswordId}", passwordId);

            var updatedPasswordDto = new PasswordDto
            {
                password = existingPassword.password,
                organization = existingPassword.organization,
                title = existingPassword.title,
                lastEdit = existingPassword.lastEdit,
                additionalFields = userInput.additionalFields
            };

            _logger.LogInfo("Password with ID: {PasswordId} successfully updated for user: {UserId}", passwordId, userId);

            return updatedPasswordDto;
        }


        public async Task DeletePasswordAsync(Guid userId, Guid passwordId)
        {
            _logger.LogInfo("Attempting to delete password with ID: {PasswordId} for user: {UserId}", passwordId, userId);

            var existingPassword = await _passwordRepository.GetOnePasswordAsync(userId, passwordId);

            if (existingPassword == null)
            {
                _logger.LogWarning("Password with ID: {PasswordId} not found for user: {UserId}", passwordId, userId);
                throw new Exception("Password not found");
            }
            try
            {
                await _passwordRepository.DeletePasswordDataAsync(passwordId);
                _logger.LogInfo("Successfully deleted password with ID: {PasswordId}", passwordId);
            }
            catch (Exception ex)
            {
                _logger.LogInfo("Error occurred while deleting password with ID: {PasswordId}. Exception: {ExceptionMessage}", passwordId, ex.Message);
                throw new Exception("Error occurred while deleting the password.");
            }
        }
    }
}

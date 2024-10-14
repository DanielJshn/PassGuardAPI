using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apief
{
    public class PassRepository : IPassRepository
    {
        private readonly DataContext _context;


        public PassRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Password password)
        {
            await _context.Passwords.AddAsync(password);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Password>> GetAllPasswordsByUserIdAsync(Guid userId)
        {
            return await _context.Passwords
                .Where(p => p.id == userId)
                .Include(p => p.additionalFields)
                .ToListAsync();
        }


        public async Task<Password?> GetOnePasswordAsync(Guid userId, Guid passwordId)
        {
            return await _context.Passwords
                .Include(p => p.additionalFields)
                .FirstOrDefaultAsync(p => p.id == userId && p.passwordId == passwordId);
        }


        public async Task RemoveAdditionalFieldsAsync(List<AdditionalField> fieldsToRemove)
        {
            _context.AdditionalFields.RemoveRange(fieldsToRemove);
            await _context.SaveChangesAsync();
        }


        public async Task AddAdditionalFieldAsync(AdditionalField field)
        {
            await _context.AdditionalFields.AddAsync(field);
            await _context.SaveChangesAsync();
        }


        public async Task DeletePasswordDataAsync(Guid passwordId)
        {
            var password = await _context.Passwords
        .FirstOrDefaultAsync(p => p.passwordId == passwordId);

            if (password != null)
            {
                _context.Passwords.Remove(password);
                await _context.SaveChangesAsync(); 
            }
        }
    }
}

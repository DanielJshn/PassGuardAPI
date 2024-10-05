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

        public async Task<Password> AddAsync(Password password)
        {
            await _context.Passwords.AddAsync(password);
            await _context.SaveChangesAsync();
            return password;
        }
    }
}

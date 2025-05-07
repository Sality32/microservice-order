using Microsoft.EntityFrameworkCore;
using UserApp.Core.Domain.Entities;
using UserApp.Core.Domain.Interfaces;
using UserApp.Infrastructure.Data.Context;

namespace UserApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Profile)
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        if (user.Profile != null)
        {
            user.Profile.UserId = user.Id;
            await _context.UserProfiles.AddAsync(user.Profile);
        }
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        
        if (user.Profile != null)
        {
            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (existingProfile != null)
            {
                _context.Entry(existingProfile).CurrentValues.SetValues(user.Profile);
            }
            else
            {
                user.Profile.UserId = user.Id;
                await _context.UserProfiles.AddAsync(user.Profile);
            }
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            if (user.Profile != null)
            {
                _context.UserProfiles.Remove(user.Profile);
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
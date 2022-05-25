using Microsoft.EntityFrameworkCore;
using SameApp.Data;
using SameApp.Models;

namespace SameApp.Services;

public class UserService
{
    private readonly SameAppContext _context;
    
    public UserService(SameAppContext context)
    {
        _context = context;
    }
    
    public async Task AddUser(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<User> GetUser(string id)
    {
        var user = await _context.User.FirstOrDefaultAsync(m => m.UserName == id);
        return user;
    }

    // Get all Items
    public async Task<List<User>> GetAllUsers()
    {
        return await _context.User.ToListAsync();
    }
    
    public async Task EditUser(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUser(string id)
    {
        var user = await _context.User.FindAsync(id);
        if (user != null)
        {
            _context.User.Remove(user);
        }
        await _context.SaveChangesAsync();
    }

    public bool IsExist(string id)
    {
        return _context.User.Any(e => e.UserName == id);
    }
    public bool IsExist(string userName, string password)
    {
        return _context.User.Any(e => e.UserName == userName && e.Password == password);
    }
}
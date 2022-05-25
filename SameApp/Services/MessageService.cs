using Microsoft.EntityFrameworkCore;
using SameApp.Data;
using SameApp.Models;

namespace SameApp.Services;

public class MessageService
{
    private readonly SameAppContext _context;
    
    public MessageService(SameAppContext context)
    {
        _context = context;
    }
    
    public async Task AddMessage(Message message)
    {
        _context.Add(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Message> GetMessage(int? id)
    {
        var message = await _context.Message.FirstOrDefaultAsync(m => m.Id == id);
        return message;
    }

    // Get all Items
    public async Task<List<Message>> GetAllMessages()
    {
        return await _context.Message.ToListAsync();
    }
    
    public  List<Message> GetAllMessages(Contact contact)
    {
        var messages =  _context.Message.Where(i => i.Contact.Id == contact.Id).ToList();
        return messages;
        //var sameAppContext = _context.Message.Include(m => m.Contact);
        //return await sameAppContext.ToListAsync();
    }
    
    public List<Message> GetAllMessages(Contact contact, string condition)
    {
        var messages =  _context.Message.Where(i => i.Contact.Id == condition).ToList();
        return messages;
    }
    
    public async Task EditMessage(Message message)
    {
        _context.Update(message);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteMessage(int id)
    {
        var message = await _context.Message.FindAsync(id);
        if (message != null)
        {
            _context.Message.Remove(message);
        }
        await _context.SaveChangesAsync();
    }

    public bool IsExist(int id)
    {
        return _context.Message.Any(e => e.Id == id);
    }
}
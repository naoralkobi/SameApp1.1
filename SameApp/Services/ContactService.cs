using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SameApp.Data;
using SameApp.Models;

namespace SameApp.Services;

public class ContactService
{
    private readonly SameAppContext _context;
    
    public ContactService(SameAppContext context)
    {
        _context = context;
    }
    
    public void AddContact(Contact item)
    {
        _context.Add(item);
        _context.SaveChanges();
    }
    
    public async Task<Contact> GetContact(string userId, string owner)
    {
        var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == userId && m.UserNameOwner == owner);
        return contact;
    }

    // Get all Items
    public async Task<List<Contact>> GetAllContacts()
    {
        return await _context.Contact.ToListAsync();
    }
    
    public List<Contact> GetAllContacts(User user)
    {
        //var contacts = _context.Contact.Include(m => m.Messages).Where(i => i.User.UserName == user.UserName).ToList();
        var contacts = _context.Contact.Where(contact => contact.UserNameOwner == user.UserName).ToList();
        //var contacts = _context.Contact.Where(contact => contact.UserNameOwner == user.UserName).Select()
        return contacts;

    }
    
    public async Task EditContact(Contact contact)
    {
        _context.Update(contact);
        await _context.SaveChangesAsync();
        
    }
    
    public async Task DeleteContact(string id, string owner)
    {
        var contact = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id && m.UserNameOwner == owner);
        if (contact != null)
        {
            _context.Contact.Remove(contact);
        }
        await _context.SaveChangesAsync();
    }

    public bool IsExist(string id)
    {
        return _context.Contact.Any(e => e.Id == id);
    }
}
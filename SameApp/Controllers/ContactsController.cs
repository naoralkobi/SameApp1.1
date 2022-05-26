using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;

        public ContactsController(ContactService service1, UserService service2)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
        }

        // GET: Contact
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserName = HttpContext.Session.GetString("username");
            if (currentUserName == null)
            {
                return NotFound();
            }

            var user = await _serviceUsers.GetUser(currentUserName);
            var contacts =  _serviceContacts.GetAllContacts(user);
            return Json(contacts);
        }

        // GET: Contact/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var contact = await _serviceContacts.GetContact(id, HttpContext.Session.GetString("username"));
            if (contact == null)
            {
                return NotFound();
            }

            return Json(contact);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,UserNameOwner,Name,Server,Last,LastDate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Messages = new List<Message>();
                
                User user = await _serviceUsers.GetUser(contact.UserNameOwner);
                
                if (user.Contacts == null)
                {
                    user.Contacts = new List<Contact>();
                }
        
                contact.User = user;
                
                user.Contacts.Add(contact);
                
                _serviceContacts.AddContact(contact);
        
                return Created(string.Format("/api/Contacts/{0}", contact.Id), contact);
            }
        
            return BadRequest();
        }
        
        // POST: Contact/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (await _serviceContacts.GetAllContacts() == null)
            {
                return Problem("Entity set 'SameAppContext.Contact'  is null.");
            }
            await _serviceContacts.DeleteContact(id);
            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> Edit([Bind("Id,UserNameOwner,Name,Server,Last,LastDate")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceContacts.EditContact(contact);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_serviceContacts.IsExist(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return BadRequest();
        }
        
        
    }
}
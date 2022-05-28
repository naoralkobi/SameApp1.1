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
        private readonly MessageService _serviceMessages;

        public ContactsController(ContactService service1, UserService service2, MessageService service3)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
            _serviceMessages = service3;
        }

        // GET: Contact
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
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
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
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
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
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
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (await _serviceContacts.GetAllContacts() == null)
            {
                return Problem("Entity set 'SameAppContext.Contact'  is null.");
            }
            // get all messages of id.
            // 1. get contact(id)
            // 2. get messages(contact)
            // 3. delete the messages.
            Contact contact = await _serviceContacts.GetContact(id, HttpContext.Session.GetString("username"));
            List<Message> messages = _serviceMessages.GetAllMessages(HttpContext.Session.GetString("username"), id);
            foreach (var message in messages)
            {
                await _serviceMessages.DeleteMessage(message.Id);
            }
            await _serviceContacts.DeleteContact(id, HttpContext.Session.GetString("username"));
            return NoContent();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([Bind("Id,UserNameOwner,Name,Server,Last,LastDate")] Contact contact)
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
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
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;
        private readonly MessageService _serviceMessages;

        public MessagesController(ContactService service1, UserService service2, MessageService service3)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
            _serviceMessages = service3;
        }

        // GET: Contact
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserName = HttpContext.Session.GetString("username");
            var currentContact = HttpContext.Session.GetString("currentContact");
            if (currentUserName == null)
            {
                return NotFound();
            }
            // from the current user we create a contact.
            var contact = await _serviceContacts.GetContact(currentUserName);
            var messages =
                _serviceMessages.GetAllMessages(contact, currentContact);
            return Ok(messages);
        }

        // GET: Contact/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!_serviceMessages.IsExist(1))
            {
                return NotFound();
            }

            var message = await _serviceMessages.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Content,Created,Sent,UserId,ContactId")] Message message1)
        {
            if (ModelState.IsValid)
            {
                Message message = new Message
                {
                    Content = message1.Content,
                    Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                    Sent = true,
                    UserId = message1.UserId,
                    ContactId = message1.ContactId,
                };
                
                
                string senderName = HttpContext.Session.GetString("username");
                
                Contact senderContact = await _serviceContacts.GetContact(senderName);
                
                if (senderContact == null)
                {
                    senderContact = new Contact();
                }
                message.Contact = senderContact;
                
                senderContact.Last = message.Content;
                senderContact.LastDate = message.Created;
                if (senderContact.Messages == null)
                {
                    senderContact.Messages = new List<Message>();
                }
                senderContact.Messages.Add(message);
                await _serviceMessages.AddMessage(message);

                return Created(string.Format("/api/Messages/{0}", message.Id), message);
            }

            return BadRequest();

        }
        
        // POST: Contact/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _serviceMessages.GetAllMessages() == null)
            {
                return Problem("Entity set 'SameAppContext.Contact'  is null.");
            }
            await _serviceMessages.DeleteMessage(id);
            return NoContent();
        }
    }

}
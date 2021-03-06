using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/Contacts/{id1}/[controller]")]
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
        
        
        // id1 is the name we get from url.
        //http://foo.com/api/contacts/:Aviv/messages ===> username = naor.
        // GET: Contact
        [HttpGet]
        public async Task<IActionResult> Index(string id1)
        {
            // if (@d("username") == null)
            // {
            //     return RedirectToAction("Login", "User");
            // }
            
            // try to get from session.
            var currentUserName = HttpContext.Session.GetString("username");
            
            // get from url.
            if (currentUserName == null)
            {
                currentUserName = HttpContext.Request.Query["username"].ToString();
            }
            
            // from the current user we create a contact.
            var user = await _serviceUsers.GetUser(currentUserName);

            var messages =
                _serviceMessages.GetAllMessages(currentUserName, id1);
            return Json(messages);
        }

        // GET: Contact/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (!_serviceMessages.IsExist(id))
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
        public async Task<IActionResult> Create([FromBody] Message message1)
        {
            // if (@HttpContext.Session.GetString("username") == null)
            // {
            //     return RedirectToAction("Login", "User");
            // }
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


                string senderName = message.ContactId;
                
                Contact senderContact = await _serviceContacts.GetContact(senderName, message.UserId);
                
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

                return Ok(message);
            }

            return BadRequest();

        }
        
        // POST: Contact/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (await _serviceMessages.GetAllMessages() == null)
            {
                return Problem("Entity set 'SameAppContext.Contact'  is null.");
            }
            await _serviceMessages.DeleteMessage(id);
            return NoContent();
        }
        
        
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Created,Sent,UserId,ContactId")] Message message)
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceMessages.EditMessage(message);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_serviceMessages.IsExist(message.Id))
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
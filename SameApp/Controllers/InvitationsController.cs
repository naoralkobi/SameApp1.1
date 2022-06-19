using Microsoft.AspNetCore.Mvc;
using SameApp.Services;
using SameApp.Models;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;

        public InvitationsController(ContactService service1, UserService service2)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
        }

        public class InvitationsData
        {
            public string From { get; set; }
            public string To { get; set; } 
            public string Server { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> RequestContact([FromBody] InvitationsData data)
        {
            if (ModelState.IsValid) {
                
                User to = await _serviceUsers.GetUser(data.To);
                if(to.Contacts == null)
                {
                    to.Contacts = new List<Contact>();
                }
                
                User from = await _serviceUsers.GetUser(data.From);

            

                Contact contactRequest = new Contact
                {
                    Server = data.Server,
                    Id = data.From,
                    Name = data.From,
                    Messages = new List<Message>(),
                    User = to,
                    UserNameOwner = data.To,
                    Last = " ",
                    LastDate = " ",
                };


                to.Contacts.Add(contactRequest);

                _serviceContacts.AddContact(contactRequest);

                return Ok();

                //return Created(string.Format("/api/invitations/{0}", data.To), data);
            }

            return BadRequest();
        }
    }
}
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SameApp.Services;
using SameApp.Models;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;
        private readonly MessageService _serviceMessages;
        private TokenData _tokenData;

        public TransferController(ContactService service1, UserService service2, MessageService serviceMessages)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
            _serviceMessages = serviceMessages;
        }

        public class TransferData
        {
            public string From { get; set; }
            public string To { get; set; } 
            public string Content { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> RequestTransfer([FromBody] TransferData data)
        {
            if (ModelState.IsValid) {
                
                Message message = new Message
                {
                    Content = data.Content,
                    Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                    UserId = data.To,
                    ContactId = data.From,
                };
                
                //##########
                string receiverName = data.From;
                message.Sent = false;
                Contact receiverContact = await _serviceContacts.GetContact(receiverName, data.To);//***
                if (receiverContact == null)
                {
                    receiverContact = new Contact();
                }
                receiverContact.Last = data.Content;
                receiverContact.LastDate = message.Created;
                if (receiverContact.Messages == null)
                {
                    receiverContact.Messages = new List<Message>();
                }
                receiverContact.Messages.Add(message);
                await _serviceMessages.AddMessage(message);
                
                
                _tokenData = TokenData.GetInstance();
                
                if (!_tokenData.GetTokens().ContainsKey(data.To))
                {
                    // is not android
                    return Ok();
                    //return Created(string.Format("/api/transfer/{0}", data.To), data);
                }
                // is android
                _tokenData.PushNotification(data.To, data.From, data.Content);
                return Ok();

            }

            return BadRequest();
        }
        
        
        
        
    }
}
using System.Diagnostics;
using System.Dynamic;
using SameApp.Models;
using SameApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace SameApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly MessageService _serviceMessages;
        private readonly UserService _serviceUsers;

        public HomeController(ContactService service1, MessageService service2, UserService service3)
        {
            _serviceContacts = service1;
            _serviceMessages = service2;
            _serviceUsers = service3;
        }
        
        public IActionResult Index()
        {
            return View(nameof(Index));
        }
        
        
        public async Task<IActionResult> Chat()
        {

             if (@HttpContext.Session.GetString("username") == null)
             {
                 return RedirectToAction("Login", "User");
             }

             
             dynamic myModel = new ExpandoObject();

             User user = await _serviceUsers.GetUser(HttpContext.Session.GetString("username"));
            
            
             //Contact contact = _serviceUsers.GetUser(HttpContext.Session.GetString("contactname"));
            
             if (user.Contacts == null)
             {
                 user.Contacts = new List<Contact>();
             }

             //myModel.Contacts = user.Contacts.ToList();
             myModel.Contacts = _serviceContacts.GetAllContacts(user);
             myModel.Messages = new List<Message>();
             myModel.Users = _serviceUsers.GetAllUsers();
            
            
            
             return View("Chat", myModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chat(string contactName)
        {
            dynamic myModel = new ExpandoObject();
            
            User user = await _serviceUsers.GetUser(HttpContext.Session.GetString("username"));

            if (contactName == null)
            {
                return  View("Chat", myModel);
            }


            Contact contact = await _serviceContacts.GetContact(contactName, HttpContext.Session.GetString("username"));
            
            HttpContext.Session.SetString("currentContact", contactName);

            HttpContext.Session.SetString("currentContactDisplay", contact.Name);
            HttpContext.Session.SetString("currentContactId", contact.Id);
            
            HttpContext.Session.SetString("currentContactServer", contact.Server);
            
            if (user.Contacts == null)
            {
                user.Contacts = new List<Contact>();
            }

            //myModel.Contacts = user.Contacts.ToList();
            myModel.Contacts = _serviceContacts.GetAllContacts(user);
            myModel.Messages = _serviceMessages.GetAllMessages(contact);

            
            return View("Chat", myModel);
        }
        

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;
        private readonly MessageService _serviceMessages;
        private TokenData _tokenData;

        public UsersController(ContactService service1, UserService service2, MessageService service3)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
            _serviceMessages = service3;
        }

        // GET: Contact
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _serviceUsers.GetAllUsers();
            return Json(users);
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _serviceUsers.IsExist(user.UserName);

                if (!q)
                {
                    await _serviceUsers.AddUser(user);
                    HttpContext.Session.SetString("username", user.UserName);
                    return Ok();
                }
                return NoContent();
            }

            return BadRequest();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _serviceUsers.IsExist(user.UserName, user.Password);
                if (q)
                {
                    HttpContext.Session.SetString("username", user.UserName);
                    return Ok();
                }
                return NotFound();
            }
            return NoContent();
        }

        // // POST: User/Delete/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteConfirmed(string id)
        // {
        //     if (@HttpContext.Session.GetString("username") == null)
        //     {
        //         return RedirectToAction("Login", "User");
        //     }
        //     if (await _serviceContacts.GetAllContacts() == null)
        //     {
        //         return Problem("Entity set 'SameAppContext.Contact'  is null.");
        //     }
        //     // get all messages of id.
        //     // 1. get contact(id)
        //     // 2. get messages(contact)
        //     // 3. delete the messages.
        //     Contact contact = await _serviceContacts.GetContact(id, HttpContext.Session.GetString("username"));
        //     List<Message> messages = _serviceMessages.GetAllMessages(HttpContext.Session.GetString("username"), id);
        //     foreach (var message in messages)
        //     {
        //         await _serviceMessages.DeleteMessage(message.Id);
        //     }
        //     await _serviceContacts.DeleteContact(id, HttpContext.Session.GetString("username"));
        //     return NoContent();
        // }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password")] User user)
        {
            if (@HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceUsers.EditUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserName))
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

            return NotFound();
        }

        private bool UserExists(string id)
        {
            return _serviceUsers.IsExist(id);
        }
        
        // GET: User/Details/5
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

            var user = await _serviceUsers.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            user.ToJson();

            return Json(user);
        }
        
        
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Token([FromBody] string userName, string token)
        {
            if (ModelState.IsValid)
            {
                _tokenData = TokenData.GetInstance();
                if (!_tokenData.GetTokens().ContainsKey(userName))
                {
                    _tokenData.AddKey(userName, token);
                    return Ok();
                }
                return NotFound();
            }
            return NoContent();
        }


    }
}
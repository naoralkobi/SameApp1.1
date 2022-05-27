using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SameApp.Data;
using SameApp.Models;
using SameApp.Services;

namespace SameApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly ContactService _serviceContacts;
        private readonly UserService _serviceUsers;

        public ContactController(ContactService service1, UserService service2)
        {
            _serviceContacts = service1;
            _serviceUsers = service2;
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            return View(nameof(Index), await _serviceContacts.GetAllContacts());
        }

        // GET: Contact/Details/5
        // public async Task<IActionResult> Details(string id)
        // {
        //     if (id == null || await _serviceContacts.GetAllContacts() == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var contact = await _serviceContacts.GetContact(id);
        //     if (contact == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(contact);
        // }

        // GET: Contact/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserNameOwner,Name,Server,Last,LastDate")] Contact contact)
        {
            if (ModelState.IsValid)
            {

                // if new contact not user dont add it or try add him self.
                if (!_serviceUsers.IsExist(contact.Id) || contact.Id == HttpContext.Session.GetString("username"))
                {
                    return RedirectToAction("Chat", "Home");
                }
                
                contact.Messages = new List<Message>();
                
                User user = await _serviceUsers.GetUser(HttpContext.Session.GetString("username"));
                
                if (user.Contacts == null)
                {
                    user.Contacts = new List<Contact>();
                }

                contact.User = user;

                contact.Server = "localhost:7001";
                
                user.Contacts.Add(contact);
                
                _serviceContacts.AddContact(contact);
                
                return RedirectToAction("Chat", "Home");
            }
            return View(contact);
        }

        // GET: Contact/Edit/5
        // public async Task<IActionResult> Edit(string id)
        // {
        //     if (id == null || await _serviceContacts.GetAllContacts() == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var contact = await _serviceContacts.GetContact(id);
        //     if (contact == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(contact);
        // }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserNameOwner,Name,Server,Last,LastDate")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceContacts.EditContact(contact);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: Contact/Delete/5
        // public async Task<IActionResult> Delete(string id)
        // {
        //     if (id == null || await _serviceContacts.GetAllContacts() == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var contact = await _serviceContacts.GetContact(id);
        //     if (contact == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(contact);
        // }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (await _serviceContacts.GetAllContacts() == null)
            {
                return Problem("Entity set 'SameAppContext.Contact'  is null.");
            }
            await _serviceContacts.DeleteContact(id, HttpContext.Session.GetString("username"));
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(string id)
        {
            return _serviceContacts.IsExist(id);
        }
        public IActionResult Ajax()
        {
            return View();
        }
    }
}

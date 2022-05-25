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
    public class MessageController : Controller
    {
        private readonly MessageService _serviceMessages;
        private readonly ContactService _serviceContacts;

        public MessageController(MessageService service1, ContactService service2)
        {
            _serviceMessages = service1;
            _serviceContacts = service2;
        }

        // GET: Message
        public async Task<IActionResult> Index()
        {
            //
            // var sameAppContext = _context.Message.Include(m => m.Contact);
            // return View(await sameAppContext.ToListAsync());
            return View(nameof(Index), await _serviceMessages.GetAllMessages());

        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _serviceMessages.GetAllMessages() == null)
            {
                return NotFound();
            }

            var message = await _serviceMessages.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            //ViewData["ContactId"] = new SelectList(_context.Contact, "Id", "Id");
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Created,Sent,UserId,ContactId")] Message message)
        {
            // if (ModelState.IsValid)
            // {
            //     _context.Add(message);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            // ViewData["ContactId"] = new SelectList(_context.Contact, "Id", "Id", message.ContactId);
            // return View(message);
            if (ModelState.IsValid)
            {
                await _serviceMessages.AddMessage(message);
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        // GET: Message/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // if (id == null || _context.Message == null)
            // {
            //     return NotFound();
            // }
            //
            // var message = await _context.Message.FindAsync(id);
            // if (message == null)
            // {
            //     return NotFound();
            // }
            // ViewData["ContactId"] = new SelectList(_context.Contact, "Id", "Id", message.ContactId);
            // return View(message);
            if (id == null || await _serviceMessages.GetAllMessages() == null)
            {
                return NotFound();
            }

            var message = await _serviceMessages.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Message/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Created,Sent,UserId,ContactId")] Message message)
        {
            //
            // if (id != message.Id)
            // {
            //     return NotFound();
            // }
            //
            // if (ModelState.IsValid)
            // {
            //     try
            //     {
            //         _context.Update(message);
            //         await _context.SaveChangesAsync();
            //     }
            //     catch (DbUpdateConcurrencyException)
            //     {
            //         if (!MessageExists(message.Id))
            //         {
            //             return NotFound();
            //         }
            //         else
            //         {
            //             throw;
            //         }
            //     }
            //     return RedirectToAction(nameof(Index));
            // }
            // ViewData["ContactId"] = new SelectList(_context.Contact, "Id", "Id", message.ContactId);
            // return View(message);
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
                    if (!MessageExists(message.Id))
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
            return View(message);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _serviceMessages.GetAllMessages() == null)
            {
                return NotFound();
            }

            var message = await _serviceMessages.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _serviceMessages.GetAllMessages() == null)
            {
                return Problem("Entity set 'SameAppContext.Message'  is null.");
            }

            await _serviceMessages.DeleteMessage(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _serviceMessages.IsExist(id);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> AddNewMessage(string content)
        {
            

            if (ModelState.IsValid)
            {
                
                Message message = new Message
                {
                    Content = content,
                    Created = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                };
                
                //##########
                string senderName = HttpContext.Session.GetString("username");
                message.Sent = true;
                Contact senderContact = await _serviceContacts.GetContact(senderName);
                if (senderContact == null)
                {
                    senderContact = new Contact();
                }
                senderContact.Last = content;
                senderContact.LastDate = message.Created;
                if (senderContact.Messages == null)
                {
                    senderContact.Messages = new List<Message>();
                }
                senderContact.Messages.Add(message);
                await _serviceMessages.AddMessage(message);

                //##########
                string receiverName = HttpContext.Session.GetString("currentContact");
                message.Sent = false;
                message.Id += 1;
                Contact receiverContact = await _serviceContacts.GetContact(receiverName);
                if (receiverContact == null)
                {
                    receiverContact = new Contact();
                }
                receiverContact.Last = content;
                receiverContact.LastDate = message.Created;
                if (receiverContact.Messages == null)
                {
                    receiverContact.Messages = new List<Message>();
                }
                receiverContact.Messages.Add(message);
                await _serviceMessages.AddMessage(message);
            }
            return RedirectToAction("Chat", "Home");
            
        }
        
        public IActionResult Ajax()
        {
            return View();
        }
    }
}

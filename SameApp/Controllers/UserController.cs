
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
    public class UserController : Controller
    {
        private readonly UserService _serviceUsers;

        public UserController(UserService service1)
        {
            _serviceUsers = service1;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(nameof(Index), await _serviceUsers.GetAllUsers());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || await _serviceUsers.GetAllUsers() == null)
            {
                return NotFound();
            }

            var user = await _serviceUsers.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {

                var q = _serviceUsers.IsExist(user.UserName);

                if (q)
                {
                    ViewData["Error"] = "user name already exist.";
                }
                else
                {
                    await _serviceUsers.AddUser(user);
                    HttpContext.Session.SetString("username", user.UserName);
                    return RedirectToAction("Chat", "Home");
                }
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || await _serviceUsers.GetAllUsers() == null)
            {
                return NotFound();
            }

            var user = await _serviceUsers.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Password")] User user)
        {
            if (id != user.UserName)
            {
                return NotFound();
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
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || await _serviceUsers.GetAllUsers() == null)
            {
                return NotFound();
            }

            var user = await _serviceUsers.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (await _serviceUsers.GetAllUsers() == null)
            {
                return Problem("Entity set 'SameAppContext.User'  is null.");
            }

            await _serviceUsers.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _serviceUsers.IsExist(id);
        }
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _serviceUsers.IsExist(user.UserName, user.Password);
                if (q)
                {
                    
                    //return RedirectToAction(nameof(Index));
                    HttpContext.Session.SetString("username", user.UserName);
                    return RedirectToAction("Chat", "Home");
                }
                else
                {
                    ViewData["Error"] = "Username and/or Password are incorrect.";
                }
            }
            return View(user);
        }
    }
}

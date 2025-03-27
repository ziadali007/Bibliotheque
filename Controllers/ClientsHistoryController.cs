using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.Extensions.Hosting;

namespace bibliotheque.Controllers
{
    public class ClientsHistoryController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ClientsHistoryController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public IActionResult ClientsHistory()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public async Task<IActionResult> ClientsHistoryViewAsync(string cat)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.MyString = cat;

            var client = _context.Clients.ToList();
            ViewBag.try_client = client; 

            return _context.Operations != null ?

            View(await _context.Operations.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Books'  is null.");
        }
    }
}

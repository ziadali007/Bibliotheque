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
    public class ReturnController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ReturnController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Return()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            return _context.Operations != null ?
            View(await _context.Operations.ToListAsync()) :
            Problem("Entity set 'ApplicationDbContext.Books'  is null.");
        }

        public async Task<IActionResult> ReturnBook(int id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (_context.Operations == null)
            {
                return NotFound();
            }

            var oper = await _context.Operations.FirstOrDefaultAsync(m => m.operation_id == id);
            string book_name = oper.book_name;
            var book = await _context.Books.FirstOrDefaultAsync(m => m.book_name == book_name);
            if (book != null) {
                book.quantity++;
            }

            if (oper != null)
            {
                oper.type = "borrowed";
            }

            Operation op = new Operation();
            op.book_name = oper.book_name;
            op.date = DateTime.Now;
            op.type = "return";
            op.quantity = 1;
            op.national_id = oper.national_id;
            _context.Add(op);
            _context.SaveChanges();

            return View("Views/Home/Success.cshtml");
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.Extensions.Hosting;

namespace bibliotheque.Controllers
{
	public class BookEntryController : Controller
	{
		    private ApplicationDbContext db;
			private readonly IWebHostEnvironment _environment;

			 public BookEntryController(ApplicationDbContext db, IWebHostEnvironment environment)
			{
				this.db = db;
				 _environment = environment;
			}

        public IActionResult BookEntry()
		{
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
		}

		public async Task<IActionResult> OldBookEntryAsync(string book_name, int quantity)
		{

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await db.Books.FirstOrDefaultAsync(m => m.book_name == book_name);

            if (book_name != null && book != null && quantity > 0)
            {
                book.quantity += quantity;
                db.SaveChanges();
                return View("Views/Home/Success.cshtml");
            }
            var books = db.Books.ToList();
            return View(books);
        }
		[HttpGet]
		public IActionResult NewBookEntry()
		{
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new Book());
		}

		[HttpPost]
		public async Task<IActionResult> NewBookEntryAsync(Book book, IFormFile book_pic)
		{

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            string path = Path.Combine(_environment.WebRootPath, "Img");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (book_pic != null)
            {
                path = Path.Combine(path, book_pic.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await book_pic.CopyToAsync(stream);
                    ViewBag.Message = string.Format("<b>{0}</b> uploaded.</br>", book_pic.FileName.ToString());
                }
                book.book_pic = book_pic.FileName;
            }
            else
            {
                book.book_pic = "default.jpg"; 
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Add(book);
                    db.SaveChanges();
                    return View("Views/Home/Success.cshtml");
                }
            
            }
            catch (Exception ex) { ViewBag.exc = ex.Message; }

            return View(book);
        }

    }
}


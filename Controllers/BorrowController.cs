using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Controllers
{
	public class BorrowController : Controller
	{

		private ApplicationDbContext db;
		public BorrowController(ApplicationDbContext db)
		{
			this.db = db;
		}
        public IActionResult Borrow(string book_name) 
		{

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (book_name != null) {
                    return RedirectToAction("Create_borrow", "ClientInformation", new {
                    book_name = book_name,
                   // fine_per_day = fine_per_day,
                    date = DateTime.Today,
                    type = "borrow",
                     quantity = 1});
            }

			var books = db.Books.ToList();
			return View(books);
		}


        public IActionResult GetBookQuantity(string book_name)
        {
            var book = db.Books.FirstOrDefault(b => b.book_name == book_name);
            if (book != null)
            {
                return Content(book.quantity.ToString());
            }
            else
            {
                return Content("");
            }
        }
    }
}

using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Controllers
{
    public class BuyController : Controller
    {

        private ApplicationDbContext db;
        public BuyController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> BuyAsync(string book_name, int ordered_quantity)
        {

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await db.Books.FirstOrDefaultAsync(m => m.book_name == book_name);

            if (book_name != null && book != null && ordered_quantity <= book.quantity && ordered_quantity !=0 )
            {
                return RedirectToAction("Create_buy", "ClientInformation", new
                {
                    book_name = book_name,
                    date = DateTime.Today,
                    type = "buy",
                    quantity = ordered_quantity
                });
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
        public IActionResult GetBookPrice(string book_name)
        {
            var book = db.Books.FirstOrDefault(b => b.book_name == book_name);
            if (book != null)
            {
                return Content(book.price.ToString());
            }
            else
            {
                return Content("");
            }
        }

        public IActionResult GetBookTotalPrice(string book_name, int quantity)
        {
            var book = db.Books.FirstOrDefault(b => b.book_name == book_name);
            if (book != null)
            {
                int total = book.price * quantity;
                return Content(total.ToString());
            }
            else
            {
                return Content("");
            }
        }

        [HttpPost]
        public IActionResult toCIN(string book_name, int price, int available_quantity, int ordered_quantity, int total_price)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (ordered_quantity <= available_quantity && ordered_quantity > 0)
            {
                return View("Views/ClientInformation/ClientInformation.cshtml");
            }
            else {
                return RedirectToAction("Buy");
            }
        }
    }
}

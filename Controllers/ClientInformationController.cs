using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Controllers
{
    public class ClientInformationController : Controller
    {
        private ApplicationDbContext db;
        public ClientInformationController(ApplicationDbContext db)
        {
            this.db = db;
        }

		public IActionResult ClientInformation()
		{
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
		}

		public IActionResult Create_borrow(string book_name , DateTime date , string type , int quantity) {

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            TempData["book_name"] = book_name;
            TempData["date"] = date;
            TempData["quantity"] = quantity;
            TempData["type"] = type;

            return View("Views/ClientInformation/ClientInformation.cshtml");  
        }

        public IActionResult Create_buy(string book_name, DateTime date, string type, int quantity)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            TempData["book_name"] = book_name;
            TempData["date"] = date;
            TempData["quantity"] = quantity;
            TempData["type"] = type;

            return View("Views/ClientInformation/ClientInformation.cshtml");
        }

        [HttpPost]
		public IActionResult MakeOperation(string client_name , string national_id , 
            string telephone, string email_address , string home_address )
        {

            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            var b1 = db.Clients.FirstOrDefault(u => u.national_id == national_id);

            if (b1 == null)
            {
                Client cl = new Client();
                cl.national_id = national_id;
                cl.client_name = client_name;
                cl.telephone = telephone;
                cl.email_address = email_address;
                cl.home_address = home_address;

                db.Add(cl);
            }



            Operation op = new Operation();
			op.book_name = TempData["book_name"] as string;
            op.date = (DateTime)TempData["date"];
            op.quantity = (int)TempData["quantity"];
            op.type = TempData["type"] as string;
            op.national_id = national_id;
            var book = db.Books.FirstOrDefault(b => b.book_name == op.book_name);


            db.Add(op);


            if (book != null)
            {
                book.quantity -= op.quantity;
                if (book.quantity == 0) {
                    db.Books.Remove(book);
                }
            }

            db.SaveChanges();
			return View("Views/Home/Success.cshtml");
        }




    }
}

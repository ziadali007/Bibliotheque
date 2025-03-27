 using bibliotheque.Data;
using bibliotheque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Controllers
{
    public class Accountcontroller : Controller
    {
        private ApplicationDbContext db;
        public Accountcontroller(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterModel reg)
        {
            var em =  db.Accounts.FirstOrDefault(m => m.EmailAddress == reg.EmailAddress);

            if (ModelState.IsValid && em==null)
            {
                db.Add(reg);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");

            }
            return View(reg);
        }

        [HttpPost]
        public IActionResult SignUp()
        {
            return RedirectToAction("Register", "Account");
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Login(LoginModel log)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.FirstOrDefault(m => m.EmailAddress==log.EmailAddress);

                if(user != null && user.Password == log.Password) {
                    var isAdmin = user.isAdmin;

                    HttpContext.Session.SetString("Name", user.fullName);
                    if (isAdmin == true)
                    {
                        HttpContext.Session.SetString("isAdmin", "yes");
                        return RedirectToAction("Index", "Home");
                    }
                    else {
                        HttpContext.Session.SetString("isAdmin", "no");
                        return RedirectToAction("Show", "Book");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email or Passward");
                }

            }
            return View(log);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Name");
            return RedirectToAction("Login", "Account");
        }
    }
}

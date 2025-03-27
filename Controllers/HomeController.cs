using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using bibliotheque.Models;
using Microsoft.EntityFrameworkCore;
using bibliotheque.Data;

namespace bibliotheque.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public HomeController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;

    }

    public IActionResult Index()
    {
        var name = HttpContext.Session.GetString("Name");
        var isAdmin = HttpContext.Session.GetString("isAdmin");

        if (string.IsNullOrEmpty(name) || isAdmin == "no")
        {
            return RedirectToAction("Login", "Account");
        }
        return View();
    }

    public IActionResult Index2()
    {
        var name = HttpContext.Session.GetString("Name");
        var isAdmin = HttpContext.Session.GetString("isAdmin");

        if (string.IsNullOrEmpty(name))
        {
            return RedirectToAction("Login", "Account");
        }

        var books = _context.Books.ToList();
        ViewBag.booklist = books;
        return View();
    }

    public IActionResult Success()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


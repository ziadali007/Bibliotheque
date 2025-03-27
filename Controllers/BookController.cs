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
using Cart_biblio.Models;
using System.Net.Mail;

namespace bibliotheque.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BookController(ApplicationDbContext context , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
               
        }

        // GET: Book    

        public async Task<IActionResult> ShowAsync()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Account");
            }

            return _context.Books != null ?
                          View(await _context.Books.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Books'  is null.");
        }

        public async Task<IActionResult> addtocart(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Account");
            }
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("cart") ?? new List<CartItem>();
            CartItem Item1 = cart.FirstOrDefault(u => u.book_name == id);
            Book b1 = _context.Books.FirstOrDefault(u => u.book_name == id);

            if (Item1 != null && Item1.Quantity < b1.quantity)
            {
           
                Item1.Quantity++;

                Item1.totalPrice += b1.price;
            }
            else if(Item1 == null)
            {
        
                CartItem newItem = new CartItem
                {
                    book_name = b1.book_name,
                    totalPrice = b1.price 
                };
                newItem.Quantity++;
                cart.Add(newItem);    
            }
            HttpContext.Session.Set("cart", cart);

            return RedirectToAction("Show","Book");
        }

        public async Task<IActionResult> Cart()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Account");
            }
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("cart") ?? new List<CartItem>();
            return View(cart);
        }

        public IActionResult RemoveCart(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Account");
            }
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("cart") ?? new List<CartItem>();
            CartItem item = cart.Find(i => i.book_name == id);
             cart.Remove(item);
            HttpContext.Session.Set("cart", cart);
            return RedirectToAction("Cart","Book");
        }

        public IActionResult CheckOut()
        {
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("cart") ?? new List<CartItem>();
            string name = HttpContext.Session.GetString("Name");
            RegisterModel b1 = _context.Accounts.FirstOrDefault(u => u.fullName == name);
            foreach (CartItem Item in cart)
            {
                var brid = new Bridge
                {
                    book_name = Item.book_name,
                    nationalId = b1.nationalId
                };

                var v1 = _context.Bridges.Find(brid.book_name,brid.nationalId);

                if(v1 == null )
                {
                    _context.Bridges.Add(brid);
                }
            }

            var cl = _context.Clients.FirstOrDefault(u => u.national_id == b1.nationalId);

            if (cl == null)
            {
                Client client = new Client();
                client.national_id = b1.nationalId;
                client.client_name = b1.fullName;
                client.home_address = b1.homeAddress;
                client.telephone = b1.telephone;
                client.email_address = b1.EmailAddress;
                _context.Clients.Add(client);
            }


            foreach (CartItem Item in cart)
            {
                Operation oper = new Operation();
                oper.book_name = Item.book_name;
                oper.national_id = b1.nationalId;
                oper.quantity = Item.Quantity;
                oper.type = "buy_online";
                oper.date = DateTime.Today;
                _context.Operations.Add(oper);

                var book = _context.Books.FirstOrDefault(u => u.book_name == oper.book_name);
                if (book != null)
                {
                    book.quantity -= oper.quantity;
                    if (book.quantity == 0)
                    {
                        _context.Books.Remove(book);
                    }
                }
            }
        
            _context.SaveChanges();

            HttpContext.Session.Remove("cart");
            return RedirectToAction("Show","Book");
        }

        public IActionResult userHistory()
        {

            string name = HttpContext.Session.GetString("Name");
            RegisterModel b1 = _context.Accounts.FirstOrDefault(u => u.fullName == name);

            List<Bridge> brlist = _context.Bridges.Where(u => u.nationalId == b1.nationalId).ToList();

            List<Book> user_books = new List<Book>();   
            foreach (Bridge b in brlist)
            {
                Book book = _context.Books.FirstOrDefault(i => i.book_name == b.book_name);
                user_books.Add(book);
            }

            return View(user_books);
        }

        public async Task<IActionResult> Index()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            return _context.Books != null ? 
                          View(await _context.Books.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Books'  is null.");
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.book_name == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("book_name")] Book book,IFormFile book_pic)
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
                }
            }

            return View();
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("book_name,author,quantity,price,borrowable,book_pic")] Book book)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != book.book_name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.book_name))
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
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.book_name == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var name = HttpContext.Session.GetString("Name");
            var isAdmin = HttpContext.Session.GetString("isAdmin");

            if (string.IsNullOrEmpty(name) || isAdmin == "no")
            {
                return RedirectToAction("Login", "Account");
            }

            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
          return (_context.Books?.Any(e => e.book_name == id)).GetValueOrDefault();
        }
    }
}

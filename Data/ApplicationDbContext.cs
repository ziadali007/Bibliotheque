using bibliotheque.Models;
using Cart_biblio.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
		{

		}

		public DbSet<Client> Clients { get; set; }	
		public DbSet<Operation> Operations { get; set; }
		public DbSet<Book> Books { get; set; }
        public DbSet<RegisterModel> Accounts { get; set; }
        public DbSet<Bridge> Bridges { get; set; }

    }
}

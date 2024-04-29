using ApiProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProductos.Data
{
    public class ApplicationDbContext : DbContext
    {
        //constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        { 
        }
        //Se agregan todos los modelos aqui necesariamente
        public DbSet<Product> Product { get; set; }

        internal int SavedChanges()
        {

            return base.SaveChanges();
        }
    }
}

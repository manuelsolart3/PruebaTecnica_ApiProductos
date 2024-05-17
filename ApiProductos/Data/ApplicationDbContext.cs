using ApiProductos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiProductos.Data
{

    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        //constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        { 
        }
        //Creamos el ModelCreate
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        //Se agregan todos los modelos aqui necesariamente
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AppUser> AppUser{ get; set; }

        internal int SavedChanges()
        {

            return base.SaveChanges();
        }
    }
}

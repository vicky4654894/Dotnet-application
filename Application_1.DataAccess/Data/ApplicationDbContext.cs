using Microsoft.EntityFrameworkCore;
using Application_1.Models.Models;

namespace Application_1.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor required for DI
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets (Tables)
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{

            modelBuilder.Entity<Product>().
                HasIndex(p => p.Title)
                .IsUnique();

            modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

            

        }

       
    

    }
}

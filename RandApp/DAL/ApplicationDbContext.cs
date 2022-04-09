using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RandApp.Models;

namespace RandApp.DAL
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> ItemsTable { get; set; }
        public DbSet<User> UsersTable { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<ItemColors> ItemColors { get; set; }
        public DbSet<ItemSizes> ItemSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(o => o.Color)
                .WithOne(o => o.Item)
                .OnDelete(DeleteBehavior.ClientCascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

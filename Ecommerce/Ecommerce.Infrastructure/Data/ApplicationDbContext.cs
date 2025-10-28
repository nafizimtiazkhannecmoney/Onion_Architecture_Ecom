using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<OrderDetails> OrderDetails { get; set; }
        DbSet<OrderProduct> OrderProducts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // external provider logins primary key
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(login => new { login.LoginProvider, login.ProviderKey });
            });

            // join table for many-to-many relationship between users and roles
            builder.Entity<IdentityUserRole<string>>(entity => 
            {
                entity.HasKey(role => new {role.UserId, role.RoleId});
            });

            // primary key for user tokens
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.HasKey(claim => claim.Id);
            });
        }
    }
}

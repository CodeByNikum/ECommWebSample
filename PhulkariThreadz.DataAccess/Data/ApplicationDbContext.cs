using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhulkariThreadz.Models;

namespace PhulkariThreadz.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }    
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<BannerImages> BannerImages { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<UserWishList> UserWishLists { get; set; }

    }
}

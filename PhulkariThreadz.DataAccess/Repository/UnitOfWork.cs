using PhulkariThreadz.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            SubCategory = new SubCategoryRepository(_db);
            Product = new ProductRepository(_db);
            ProductImages = new ProdImagesRepository(_db);
            UserAddresses = new UserAddressRepository(_db);
            UserDetails = new UserDetailRepository(_db);
            BannerImages = new BannerImageRepository(_db);
            ProductRatings = new ProductRatingRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Wishlist = new WishListRepository(_db);

        }
        public ICategoryRepository Category { get; private set; }
        public ISubCategoryRepository SubCategory { get; private set; }
        public IProductRepository Product { get; private set; }
        public IProdImagesRepository ProductImages { get; private set; }
        public IUserAddressRepository UserAddresses { get; private set; }
        public IUserDetailRepository UserDetails { get; private set; }
        public IBannerImageRepository BannerImages { get; private set; }
        public IProductRatingRepository ProductRatings { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IWishlistRepository Wishlist { get; private set; }
        public void Save()
        {   
            _db.SaveChanges();
        }
    }
}

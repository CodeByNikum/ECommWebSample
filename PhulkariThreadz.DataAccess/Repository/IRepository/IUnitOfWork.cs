using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ISubCategoryRepository SubCategory { get; }
        IProductRepository Product { get; }
        IProdImagesRepository ProductImages { get; }
        IUserAddressRepository UserAddresses { get; }
        IUserDetailRepository UserDetails { get; }
        IBannerImageRepository BannerImages { get;  }
        IProductRatingRepository ProductRatings { get;  }
        IShoppingCartRepository ShoppingCart { get;  }
        IWishlistRepository Wishlist { get;  }
        void Save();

    }
}

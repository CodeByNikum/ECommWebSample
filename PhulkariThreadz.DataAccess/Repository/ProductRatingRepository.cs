using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class ProductRatingRepository : Repository<ProductRating>, IProductRatingRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRatingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProductRating obj)
        {
            _db.ProductRatings.Update(obj);
        }
    }
}

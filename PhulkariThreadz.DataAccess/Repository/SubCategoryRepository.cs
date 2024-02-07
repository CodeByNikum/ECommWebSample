using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        private ApplicationDbContext _db;
        public SubCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        void ISubCategoryRepository.Update(SubCategory obj)
        {
            _db.SubCategories.Update(obj);
        }
    }
}

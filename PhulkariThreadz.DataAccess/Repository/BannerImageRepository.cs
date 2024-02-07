using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class BannerImageRepository : Repository<BannerImages>, IBannerImageRepository
    {
        private ApplicationDbContext _db;
        public BannerImageRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }

        public void Update(BannerImages obj)
        {
            _db.BannerImages.Update(obj);
        }


    }
}

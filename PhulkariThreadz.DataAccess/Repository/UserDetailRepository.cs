using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class UserDetailRepository : Repository<UserDetail>, IUserDetailRepository
    {
        private ApplicationDbContext _db;
        public UserDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
     
        public void Update(UserDetail obj)
        {
            _db.UserDetails.Update(obj);
        }
    }
   
}

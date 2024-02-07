using PhulkariThreadz.DataAccess.Repository.IRepository;
using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository
{
    public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
    {
        private ApplicationDbContext _db;
        public UserAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update (UserAddress obj)
        {
            _db.UserAddresses.Update(obj);
        }
        public void BulkUpdate(List<UserAddress> List)
        {
            foreach (var obj in List)
            {
                _db.UserAddresses.Update(obj);
            }
            
        }
    }
}

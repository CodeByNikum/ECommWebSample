using PhulkariThreadz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.DataAccess.Repository.IRepository
{
    public interface IWishlistRepository : IRepository<UserWishList>
    {
        void Update(UserWishList obj);

    }
}

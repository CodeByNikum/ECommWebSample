using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models.ViewModels
{
    public class CategoryHomeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public List<SubCategory> SubCategories { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models.ViewModels
{
    public class ProductsHomeVM
    {
        public int ProductId { get; set; }
       
        public string ProductName { get; set; }
       
        public string ProductDescription { get; set; }
        public Decimal Price { get; set; }

        public string MainImageUrl { get; set; }
        public int SubCategoryId { get; set; }
      
        //public List<SubCategory> SubCategory { get; set; }

        public List<CategoryHomeVM> Categories { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        public Decimal RatingCount { get; set; }
        public int TotalCount { get; set; }
    }
}

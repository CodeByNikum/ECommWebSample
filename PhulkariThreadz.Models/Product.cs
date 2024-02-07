using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        public Decimal Price { get; set; }

        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        [ValidateNever]
        public virtual SubCategory SubCategory { get; set; }
    }


    public class ProductRating
    {
        [Key]
        public int RecordId { get; set; }
        public int ProductId { get; set; }
        public Decimal RatingCount { get; set; }
    }

}

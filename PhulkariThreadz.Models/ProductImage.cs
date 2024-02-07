using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImgId { get; set; }

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public Boolean IsMainImage { get; set; }
    }
}

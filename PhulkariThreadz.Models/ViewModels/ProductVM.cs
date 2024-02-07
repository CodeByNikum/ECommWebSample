using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        public string MainImageUrl { get; set; }

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
        [ValidateNever]

        public IFormFile File { get; set; }

        public Decimal RatingCount { get; set; }
        

    }
}

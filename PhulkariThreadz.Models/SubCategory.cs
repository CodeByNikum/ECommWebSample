using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; }
        [Required]
        [Display(Name ="Sub Category Name")]
        public string SubCategoryName { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models
{
    public class UserAddress
    {
        [Key]
        public int UserAddress_Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public Int32 PinCode { get; set; }
        public bool IsDefault { get; set; }
        public virtual string Id { get; set; }
        public IdentityUser IdentityUser { get; set; }

    }
}

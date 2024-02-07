using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhulkariThreadz.Models
{
    public class BannerImages
    {
        [Key]
        public int BannerImageId { get; set; }
        public string ImageURl { get; set; }
        public string BannerText { get; set; }
        public string BannerSubText { get; set; }
        public string Link { get; set; }
        public string BannerTextFontColor { get; set; }
        public string BannerSubTextFontColor { get; set; }
        public int DisplayOrder { get; set; }


    }
}

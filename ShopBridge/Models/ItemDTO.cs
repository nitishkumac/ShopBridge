using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class ItemDTO
    {
        
        public int id { get; set; }
        [Required]
        [Display(Name = "Name of the Product")]
        public string name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal price { get; set; }
    }
}
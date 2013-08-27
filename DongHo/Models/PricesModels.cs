using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class PricesModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int PriceFrom { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int PriceTo { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Ord { get; set; }
        public int Active { get; set; }
    }
}
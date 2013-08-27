using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class BrandModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Name { get; set; }
        public string Logo { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public int Ord { get; set; }
        public string Lang { get; set; }
    }
}
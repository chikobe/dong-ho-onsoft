using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class SupplierModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$", ErrorMessage = "Không đúng định dạng email")]
        public string Email { get; set; }
        public string Website { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Contact { get; set; }
        public string National { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Ord { get; set; }
    }
}
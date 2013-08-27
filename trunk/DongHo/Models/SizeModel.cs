using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class SizeModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string Name { get; set; }
        public string Des { get; set; }
        public string Lang { get; set; }
    }
       
}
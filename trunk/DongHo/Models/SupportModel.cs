using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DongHo.Models
{
    public class SupportModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        public string Tel { get; set; }
        public int Type { get; set; }
        public string Nick { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Ord { get; set; }
        public int Active { get; set; }
        public int GroupSupportId { get; set; }
        public string Lang { get; set; }
        public int Location { get; set; }
    }
}
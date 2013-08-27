using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class GroupLibraryModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Level { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Ord { get; set; }
        public int Active { get; set; }
        public string Lang { get; set; }
    }
}
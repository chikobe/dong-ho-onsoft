using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class GroupNewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Không được bỏ trống")]
        public string Tag { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public int Priority { get; set; }
        public int Index { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public int Ord { get; set; }
        public string Active { get; set; }
        public string Lang { get; set; }


    }
}
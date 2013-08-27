using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class CategoryModels
    {
        public int id { get; set; }
        public string Tag { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Name { get; set; }
        public string Content { get; set; }
        public int Level { get; set; }
        public int Priority { get; set; }
        public int Index { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public int Active { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống")]
        public int Ord { get; set; }
        public string Lang { get; set; }
    }
}
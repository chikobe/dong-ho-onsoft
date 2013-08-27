using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class PageModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
        public string Detail { get; set; }
        public string Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public int Type { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Link { get; set; }
        public string Target { get; set; }
        public int Index { get; set; }
        public int Position { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Ord { get; set; }
        public int Active { get; set; }
        public string Lang { get; set; }
    }
}
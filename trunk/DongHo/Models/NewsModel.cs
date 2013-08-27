using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class NewsModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public int Priority { get; set; }
        public int Index { get; set; }
        public int Active { get; set; }
        public int GroupNewsId { get; set; }
        public string Lang { get; set; }
        
    }
}
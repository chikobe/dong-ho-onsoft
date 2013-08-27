using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class AdvertiseModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Chỉ được nhập số")]
        public int Width { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Chỉ được nhập số")]
        public int Height { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Link { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }
        public int Vip { get; set; }
        public int Click { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Chỉ được nhập số")]
        public int Ord { get; set; }
        public int Active { get; set; }
        public string Target { get; set; }
    }
}
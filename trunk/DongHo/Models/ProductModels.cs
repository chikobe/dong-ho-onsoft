using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class ProductModels
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string Name { get; set; }
        public string Content { get; set; }
        public string Detail { get; set; }
        public int Priority { get; set; }
        public int Index { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public int Price { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public int CatId { get; set; }
        public string CatTag { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public int Active { get; set; }
        public int Ord { get; set; }
        public string Lang { get; set; }
        public int BrandId { get; set; }
        public string PiceOld { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Codepro { get; set; }
        public int Count { get; set; }
        //[DisplayFormat(DataFormatString="dd/MM/yyyy")]
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int View { get; set; }
    }
}
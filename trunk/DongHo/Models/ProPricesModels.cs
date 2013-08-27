using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class ProPricesModels
    {
        public int Id { get; set; }
        public int ProId { get; set; }
        public string PriceImport { get; set; }
        public string PriceExport { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string GiaBanSi { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string GiaBanLe { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string PricePromotion { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public DateTime DateBegin { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public DateTime DateEnd { get; set; }
        public int Ord { get; set; }
        public DateTime Date { get; set; }
    }
}
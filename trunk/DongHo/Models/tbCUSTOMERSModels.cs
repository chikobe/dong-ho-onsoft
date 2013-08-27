using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class tbCUSTOMERSModels
    {
        public int iusid { get; set; }
        public string vusername { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
        public string vpassword { get; set; }
        [Required(ErrorMessage="Không được để trống")]
        public string vcusname { get; set; }
        public string dbirthday { get; set; }
        public string vprovince { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string vaddress { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string vphone { get; set; }
        public string vmobile { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$", ErrorMessage = "Không đúng định dạng email")]
        public string vemail { get; set; }
        public string dcreatedate { get; set; }
        public int istatus { get; set; }
    }
}
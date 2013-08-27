using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DongHo.ViewModels
{
    public class CustomerForm
    {
        public class Dangky
        {
            [Required(ErrorMessage="Không được để trống")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [RegularExpression(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$", ErrorMessage = "Không đúng định dạng email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Pass { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [DataType(DataType.Password)]
            [Compare("Pass", ErrorMessage = "Mật khẩu và mật khẩu nhập lại không khớp")]
            public string RePass { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            public string Address { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            public string Captcha { get; set; }
        }

        public class Dangnhap
        {
            [Required(ErrorMessage = "Không được để trống")]
            [RegularExpression(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$", ErrorMessage = "Không đúng định dạng email")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Pass { get; set; }
        }
        public class ChangePass
        {
            [Required(ErrorMessage = "Không được để trống")]
            [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
            public string Pass { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string NewPass { get; set; }
            [Required(ErrorMessage = "Không được để trống")]
            [StringLength(100, ErrorMessage = " {0} phải có ít nhất {2} kí tự.", MinimumLength = 6)]
            [Compare("NewPass", ErrorMessage = "Mật khẩu mới và mật khẩu nhập lại không khớp")]
            [DataType(DataType.Password)]
            public string ReNewPass { get; set; }
        }
    }
}
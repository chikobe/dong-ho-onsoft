using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DongHo.Models
{
    public class ConfigModels
    {
        public int Id { get; set; }
        public string Mail_Smtp { get; set; }
        public int Mail_Port { get; set; }
        public string Mail_Info { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string Mail_Noreply { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        public string Mail_Password { get; set; }
        public string Copyright { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
    }
}
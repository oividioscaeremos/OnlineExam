using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineSinav.ViewModels
{
    public class AuthLogin
    {
        [Display(Name ="Numara")]
        [Required(ErrorMessage ="Numara Alanı Boş Bırakılamaz.")]
        public string school_number { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        public string password { get; set; }
    }
}
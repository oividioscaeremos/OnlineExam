using OnlineSinav.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OnlineSinav.ViewModels
{
    public class ViewAdmin
    {
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Girişte Kullanılacak Numara")]
        public string school_number { get; set; }

        [Display(Name = "Ad Soyad")]
        public string firstname_lastname { get; set; }

        public IEnumerable<Users> allAdmins { get; set; }



    }
    public class ViewTeacher
    {
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Girişte Kullanılacak Numara")]
        public string school_number { get; set; }

        [Display(Name = "Ad Soyad")]
        public string firstname_lastname { get; set; }

        public IEnumerable<Users> allTeachers { get; set; }
    }

    public class ViewStudents
    {
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Display(Name = "Girişte Kullanılacak Numara")]
        public string school_number { get; set; }

        [Display(Name = "Ad Soyad")]
        public string firstname_lastname { get; set; }

        public IEnumerable<Users> allStudents { get; set; }
    }
    public class RolDropDown
    {
        public int id { get; set; }
        public string name { get; set; }
            }





    public class CreateUser
    {
     
        public SelectList allRoles { get; set; }
        public SelectList allDepts { get; set; }
        public string SelectedRoleID { get; set; }
        public string SelectedDeptID { get; set; }

        [Display(Name = "Kullanıcı Rolü")]
        public int userRoleID { get; set; }

        [Display(Name = "Kullanıcının Okuduğu Bölüm")]
        public int userDeptID { get; set; }

        [Display(Name = "Girişte Kullanılacak Numara")]
        [Required]
        public string school_number { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad Alanı Boş Bırakılamaz")]
        public string firstname_lastname { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Girilmeli")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Parola Tekrar")]
        [Required(ErrorMessage = "Parola Tekrarı Girilmeli")]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "Parolalar Uyuşmuyor")]
        [DataType(DataType.Password)]
        public string password_confirm { get; set; }
     

    }

    
    public class UsersEditUser
    {
        [DataType(DataType.Text)]
        [Display(Prompt = "name")]
        public string name { get; set; }
        [DataType(DataType.Text)]
        [Display(Prompt = "school_number")]
        public string school_number { get; set; }
        public IList<DropDownList> Roles { get; set; }

    }



}

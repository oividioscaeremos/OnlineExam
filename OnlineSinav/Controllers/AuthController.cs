using OnlineSinav.Models;
using OnlineSinav.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineSinav.Controllers
{
    public class AuthController : Controller
    {
        
       
        
        public ActionResult Login()
        {
               
            return View();
            
        }
        [HttpPost]
        public ActionResult Login(AuthLogin formData, string returnUrl)
        {
            var user = Database.Session.Query<Users>().FirstOrDefault(p => p.SchoolNumber == formData.school_number);

            if (user == null || !(user.CheckPassword(formData.password)))
            {
                ModelState.AddModelError("schoolNumber", "Numara veya şifre yanlış.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            FormsAuthentication.SetAuthCookie(formData.school_number, true);

            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl); 
            }

            if (user.Role.RoleName == "admin")
            {
                return RedirectToAction("Index", "Users", new { area = "Admin" });
            }
            else if (user.Role.RoleName == "student")
            {
                return RedirectToAction("Index", "Student", new {area = "Student"});
            }
            else return RedirectToAction("Index", "Exam", new { area = "Teacher" });
            
        }
        
        

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "HomePage");
        }
        }
}
using OnlineSinav.Models;
using OnlineSinav.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineSinav.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        
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

            return RedirectToAction("Index","Users",new { area = "Admin" });
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }



    }
}
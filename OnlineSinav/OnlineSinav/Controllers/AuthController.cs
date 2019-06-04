using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
        public ActionResult Login(ViewModels.AuthLogin form)
        {
            if (!ModelState.IsValid)
                return View();

            
            return View();
        }
    }
}
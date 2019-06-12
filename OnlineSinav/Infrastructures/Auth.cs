using OnlineSinav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Infrastructures
{
    public static class Auth
    {
        private const string UserKey = "OnlineSinav.Auth.UserKey";
        public static Users User
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var user = HttpContext.Current.Items[UserKey] as Users;

                if (user == null)
                {
                    user = Database.Session.Query<Users>().FirstOrDefault(p => p.SchoolNumber == HttpContext.Current.User.Identity.Name);
                }

                if (user == null)
                    return null;

                HttpContext.Current.Items[UserKey] = user;

                return user;
            }

        }
    }
}
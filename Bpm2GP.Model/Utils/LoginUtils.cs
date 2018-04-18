using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Manager;
using Bpm2GP.Model.DataBase.Models;

namespace Bpm2GP.Model.Utils
{
    public class LoginUtils
    {
        public static DbFactory DbFactory = SessionManager.Instance.DbFactory;
        
        public static User User
        {
            get
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = null;
                if (authCookie != null)
                {
                    ticket = FormsAuthentication.Decrypt(authCookie.Value);
                }

                if (ticket != null)
                {
                    return HttpContext.Current.Session["Usuario"] != null
                        ? (User) HttpContext.Current.Session["Usuario"]
                        : null;
                }

                return null;
            }
        }

        public static Designer Designer
        {
            set { HttpContext.Current.Session["Designer"] = value; }
            get
            {
                if (HttpContext.Current.Session["Designer"] == null)
                    return null;

                return (Designer)HttpContext.Current.Session["Designer"];
            }
        }

        public static User Logar(User u)
        {
            try
            {
                var user = DbFactory.UserRepository.Login(u.Password, u.UserName);
                if (user == null)
                {
                    throw new Exception("User or Password is not valid");
                }

                var designer = DbFactory.DesignerRepository.FindByUserId(user.Id);
                
                HttpContext.Current.Session["Usuario"] = user;
                HttpContext.Current.Session["Designer"] = designer;
                FormsAuthentication.SetAuthCookie(user.UserName, true);
                
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Didn't possible do the log-in. " + ex.Message);
            }
        }

        public static void Deslogar()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session["Usuario"] = null;
            HttpContext.Current.Session.Remove("Usuario");
            HttpContext.Current.Session["Designer"] = null;
            HttpContext.Current.Session.Remove("Designer");
        }
    }
}

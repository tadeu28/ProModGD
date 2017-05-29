using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;

namespace Bpm2GP.Model.Utils
{
    public class LoginUtils
    {
        private static User _user ;
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

                return ticket != null ? _user : null;
            }

            set { _user = value; }
        }

        public static User Logar(User u)
        {
            try
            {
                var user = DbFactory.Instance.UserRepository.FindAll().FirstOrDefault(f => f.Password == u.Password 
                                                                       && (f.Email == u.UserName || f.UserName == u.UserName));
                if (user == null)
                {
                    throw new Exception("User or Password is not valid");
                }
                
                FormsAuthentication.SetAuthCookie(user.UserName, true);

                User = user;
                return User;
            }
            catch (Exception ex)
            {
                throw new Exception("Didn't possible do the log-in. " + ex.Message);
            }
        }

        public static void Deslogar()
        {
            FormsAuthentication.SignOut();

            User = null;
        }
    }
}

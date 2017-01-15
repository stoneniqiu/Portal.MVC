using System;
using System.Web;
using System.Web.Security;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;

namespace Niqiu.Core.Services
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly TimeSpan _expirationTimeSpan;

      //  private User _cachedUser;

        public FormsAuthenticationService(IUserService userService)
        {
            _userService = userService;
            _expirationTimeSpan = FormsAuthentication.Timeout;
        }


        public void SignIn(User user, bool createPersistentCookie)
        {
            var now = DateTime.Now.ToLocalTime();
            var ts = new TimeSpan(7, 0, 0, 0);//7天
            if(user==null) return;
            var key = Encrypt.EncryptString(user.Username + "_" + DateTime.Now.Ticks + Encrypt.GenerateOrderNumber());
            var ticket = new FormsAuthenticationTicket(1, user.Username, now, now.Add(ts), false, key, FormsAuthentication.FormsCookiePath);
            //var ticket = new FormsAuthenticationTicket(1, user.Username, now, now.Add(_expirationTimeSpan),
            //    createPersistentCookie, user.Username, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {HttpOnly = true};
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            HttpContext.Response.Cookies.Add(cookie);
            HttpContext.Session["User"] = user;
            //nop源码中没有这一句，务必保证webconfig中的认证是form的。
            FormsAuthentication.SetAuthCookie(user.Username, createPersistentCookie);
            //_cachedUser = user;
        }

        public void SignOut()
        {
          //  _cachedUser = null;
            FormsAuthentication.SignOut();
        }

        public User GetAuthenticatedCustomer()
        {
            if (HttpContext.Session["User"]!=null)
                return HttpContext.Session["User"] as User;
            if (HttpContext == null || HttpContext.Request == null || !HttpContext.Request.IsAuthenticated ||
               !(HttpContext.User.Identity is FormsIdentity))
            {
                return null;
            }
           // if (_cachedUser != null) return _cachedUser;
            var formsIdentity = (FormsIdentity)HttpContext.User.Identity;
            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            if (user != null && user.Active && !user.Deleted )//&& user.IsRegistered()
               // _cachedUser = user;
            return user;
            return null;
        }

        public bool IsCurrentUser
        {
            get { return GetAuthenticatedCustomer() != null; }
        }

        public virtual User GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.Name;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;

            var user = _userService.GetUserByUsername(usernameOrEmail);

            return user;
        }

        public HttpContextBase HttpContext
        {
            get { return new HttpContextWrapper(System.Web.HttpContext.Current); }
        }
    }
}

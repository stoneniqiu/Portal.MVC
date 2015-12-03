using System;
using System.Web;
using Ninject;
using Niqiu.Core.Domain.Config;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Niqiu.Core.Services;

namespace Portal.MVC.Services
{
    public class WebWorkContext : IWorkContext
    {
        #region Const


        #endregion

          #region Fields

        private readonly IUserService _userService;
        private User _cachedUser;

        #endregion
        public SendMail Mail { get; set; }


        public WebWorkContext( IUserService userService )
        {
            _userService = userService;
            Mail = new SendMail();
        }


        #region Utilities

        protected virtual HttpCookie GetUserCookie()
        {
            if (HttpContext == null || HttpContext.Request == null)
                return null;

            return HttpContext.Request.Cookies[PortalConfig.UserCookieName];
        }

        protected virtual void SetUserCookie(Guid customerGuid)
        {
            if (HttpContext != null && HttpContext.Response != null)
            {
                var cookie = new HttpCookie(PortalConfig.UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 30; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                HttpContext.Response.Cookies.Remove(PortalConfig.UserCookieName);
                HttpContext.Response.Cookies.Add(cookie);
            }
        }



        #endregion


        public virtual User CurrentUser
        {
            get
            {

                User customer = AuthenticationService.GetAuthenticatedCustomer(); ;

                //load guest customer
                if (customer == null || customer.Deleted || !customer.Active)
                {
                    var customerCookie = GetUserCookie();
                    if (customerCookie != null && !String.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            var customerByCookie = _userService.GetUserByGuid(customerGuid);
                            if (customerByCookie != null &&IsCurrentUser)
                                //this customer (from cookie) should not be registered
                                //!customerByCookie.IsRegistered())
                                customer = customerByCookie;
                        }
                    }
                }


                //validation
                if (customer!=null&&!customer.Deleted && customer.Active)
                {
                    SetUserCookie(customer.UserGuid);
                }

                return customer;
                ;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }

        public User OriginalUserIfImpersonated { get; private set; }
        public bool IsAdmin { get; set; }
        public bool IsCurrentUser {
            get { return AuthenticationService.IsCurrentUser; }
        }

        public bool SendMail(string toEmails, string emailText, string subject)
        {
            if (string.IsNullOrEmpty(toEmails)) return false;
            return  Mail.SendaMail(toEmails, emailText, subject);
        }

        public void AsyncSendMail(string toEmails, string emailText, string subject)
        {
            Action action = () => Mail.SendaMail(toEmails, emailText, subject);
            action.BeginInvoke(null, action);
        }

        public HttpContextBase HttpContext
        {
            get { return new HttpContextWrapper(System.Web.HttpContext.Current); }
        }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
    }
}

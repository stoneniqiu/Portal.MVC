using System;
using System.Collections.Generic;
using System.Web;
using Ninject;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.Config;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Niqiu.Core.Services;
using Portal.MVC.Infrastructure;

namespace Portal.MVC.Services
{
    public class WebWorkContext : IWorkContext
    {
        #region Const


        #endregion

        #region Fields

        private readonly IUserService _userService;
        private readonly ICacheManager _cacheManager;
        #endregion
        public SendMail Mail { get; set; }


        public WebWorkContext(IUserService userService, ICacheManager cacheManager)
        {
            _userService = userService;
            _cacheManager = cacheManager;
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


        private User _cachedCustomer;

        public virtual User CurrentUser
        {
            get
            {
                //  if (_cachedCustomer != null) return _cachedCustomer;


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
                             customer = customerByCookie;
                        }
                    }
                }
                //需要创建一个用户了。
                //Init();

                if (customer == null || customer.Deleted || !customer.Active)
                {
                    customer = _cacheManager.Get("guest", () => _userService.InsertGuestUser());
                }

                //validation
                if (customer != null && !customer.Deleted && customer.Active)
                {
                    SetUserCookie(customer.UserGuid);
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
                ;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedCustomer = value;
            }
        }

        public User OriginalUserIfImpersonated { get; private set; }
        public bool IsAdmin { get; set; }
        public bool IsCurrentUser
        {
            get { return AuthenticationService.IsCurrentUser; }
        }

        public bool SendMail(string toEmails, string emailText, string subject)
        {
            if (string.IsNullOrEmpty(toEmails)) return false;
            return Mail.SendaMail(toEmails, emailText, subject);
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

        private void InstallRoles()
        {

            var rolist = new List<string>
            {
                SystemUserRoleNames.Administrators,
                SystemUserRoleNames.Admin,
                SystemUserRoleNames.Employeer,
                SystemUserRoleNames.Registered,
                SystemUserRoleNames.Guests,
            };
            foreach (var ro in rolist)
            {
                var role = _userService.GetUserRoleBySystemName(ro);
                if (role == null)
                {
                    role = new UserRole() { Active = true, IsSystemRole = true, Name = ro, SystemName = ro };
                    _userService.InsertUserRole(role);
                }
            }
        }



        private void Init()
        {
            InstallRoles();
            InstallAdminUser();
        }

        private void InstallAdminUser()
        {
            var user = _userService.GetUserBySystemName(SystemUserRoleNames.Administrators);
            if (user == null)
            {
                var role = _userService.GetUserRoleBySystemName(SystemUserRoleNames.Administrators);
                user = new User()
                {
                    Active = true,
                    IsSystemAccount = true,
                    UserGuid = Guid.NewGuid(),
                    Username = SystemUserRoleNames.Administrators,
                    Password = Encrypt.GetMd5Code(SystemUserRoleNames.Admin),
                    PasswordFormat = PasswordFormat.Encrypted,
                    CreateTime = DateTime.Now,
                    Description = "系统管理员",
                    SystemName = SystemUserRoleNames.Administrators
                };
                user.UserRoles.Add(role);
                _userService.InsertUser(user);
            }

        }
    }
}

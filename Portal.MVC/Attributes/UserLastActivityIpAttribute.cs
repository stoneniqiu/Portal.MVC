using System;
using System.Web.Mvc;
using Ninject;
using Niqiu.Core.Domain;
using Niqiu.Core.Services;

namespace Portal.MVC.Attributes
{
    public class UserLastActivityIpAttribute : ActionFilterAttribute
    {
        [Inject]
        public IWorkContext WorkContext { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IWebHelper WebHelper { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            //only GET requests
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var user = WorkContext.CurrentUser;
            var ip = WebHelper.GetCurrentIpAddress();
            if (user != null && user.LastActivityDateUtc.AddMinutes(1.0) < DateTime.UtcNow)
            {
                user.LastActivityDateUtc = DateTime.UtcNow;
                user.LastIpAddress = ip;
                UserService.UpdateUser(user);
            }
        }
    }
}
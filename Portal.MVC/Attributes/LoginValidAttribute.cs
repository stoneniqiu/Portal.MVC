using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Services;

namespace Portal.MVC.Attributes
{
    public class LoginValidAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     转到管理员登陆的界面
        /// </summary>
        private bool _isAdmin;

        public LoginValidAttribute(bool isadmin = false)
        {
            _isAdmin = isadmin;
        }

        [Inject]
        public IWorkContext WorkContext { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string contr = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();

            var url = filterContext.RequestContext.HttpContext.Request.FilePath;
            IDictionary<string, object> parmdatas = filterContext.ActionParameters;
            string parms = "?";
            int i = 0;
            int count = parmdatas.Count;
            foreach (var parmdata in parmdatas)
            {
                i++;
                if (i <= count - 1)
                {
                    parms += parmdata.Key + "=" + parmdata.Value + "&";
                }
                else
                {
                    parms += parmdata.Key + "=" + parmdata.Value;
                }
            }
            if (count == 0) parms = "";
            string returnUrl = string.Format("~/{0}/{1}{2}", contr, action, parms);
            returnUrl = UrlHelper.GenerateContentUrl(returnUrl, filterContext.HttpContext);

            User user = WorkContext.CurrentUser;

            if (_isAdmin)
            {
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Logon?returnUrl=" + returnUrl);
                }
                if (!WorkContext.IsAdmin)
                {
                    filterContext.Result = new RedirectResult("~/Account/Logon?returnUrl=" + returnUrl+"&msg='false'");
                }
            }

            if (user == null)
            {
                filterContext.Result = url.Contains("/Admin") ? new RedirectResult("~/Account/Logon?returnUrl=" + returnUrl) : new RedirectResult("~/Account/MLogon?returnUrl=" + returnUrl);
            }

            // 如果已经登录 但不是角色，就需要跳转到只是页面 提示是管理员才能登录
        }
    }
}
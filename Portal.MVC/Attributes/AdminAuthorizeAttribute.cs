using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Niqiu.Core.Domain.Security;
using Niqiu.Core.Services;

namespace Portal.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited=true, AllowMultiple=true)]
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _dontValidate;
        public PermissionRecord Permission
        {
            get
            {    //属性注入在构造函数中调用的时候出错。
                 var prs = PermissionService.GetAllPermissionRecords();
                 return prs.FirstOrDefault(n => n.SystemName == pname) ?? StandardPermissionProvider.AccessAdminPanel;
            }  
        }

        private string pname;

        [Inject]
        public IPermissionService PermissionService { get; set; }


        public AdminAuthorizeAttribute()
            : this(false)
        {
            pname = StandardPermissionProvider.AccessAdminPanel.SystemName;
        }

        public AdminAuthorizeAttribute(string permissionSysname)
            : this(false)
        {
            pname = permissionSysname;
           
        }

        //不验证要这个干吗 用来测试吧
        public AdminAuthorizeAttribute(bool dontValidate)
        {
            this._dontValidate = dontValidate;
        }

        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(string.Format("~/Account/Unauthorized?name={0}&returnUrl={1}",Permission.Name,GetreturnUrl(filterContext)));
        }

        private string GetreturnUrl(AuthorizationContext filterContext)
        {
            string contr = filterContext.RouteData.Values["controller"].ToString(); ;
            string action = filterContext.RouteData.Values["action"].ToString();
            var parmdatas = filterContext.RouteData.Values;
            string parms = "?";
            var i = 0;
            var count = parmdatas.Count;
            foreach (var parmdata in parmdatas)
            {
                if (parmdata.Key != "controller" && parmdata.Key != "action")
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
            }
            if (count == 0) parms = "";

            var  returnUrl = string.Format("~/{0}/{1}{2}", contr, action, parms);
            return  UrlHelper.GenerateContentUrl(returnUrl, filterContext.HttpContext);
        }

        private IEnumerable<AdminAuthorizeAttribute> GetAdminAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true))
                .OfType<AdminAuthorizeAttribute>();
        }



        private bool IsAdminPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAdminAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_dontValidate)
                return;

            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("You cannot use [AdminAuthorize] attribute when a child action cache is active");

            if (IsAdminPageRequested(filterContext))
            {
                if (!this.HasAdminAccess(filterContext))
                    this.HandleUnauthorizedRequest(filterContext);
            }
        }

     
        public virtual bool HasAdminAccess(AuthorizationContext filterContext)
        {
            bool result = PermissionService.Authorize(Permission);
            return result;
        }
    }
}

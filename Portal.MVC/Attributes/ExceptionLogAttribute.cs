using System;
using System.Web.Mvc;

namespace Portal.MVC.Attributes
{
   /// <summary>
   /// 记录异常信息 并返回一个友好界面
   /// </summary>
   public class ExceptionLogAttribute:FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var cname = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            var message = string.Format("控制器：{0} 方法：{1} ,执行时间：{2}，错误信息：{3}", cname, actionName, DateTime.Now, filterContext.Exception.Message);
            Logger.Error(message);// 没有写下去 需要再添加Logger
            //filterContext.Result = new RedirectResult("~/SpecialErrorPage.html");
            // 这个页面是在web项目中。
        }
    }
}

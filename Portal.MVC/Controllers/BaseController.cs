using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.Config;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Portal.MVC.Attributes;
using Portal.MVC.Models;

namespace Portal.MVC.Controllers
{
    [UserLastActivityIp]
    public class BaseController : Controller 
    {
        private User _curUser;
        private int _userId;

        public int CheckValid()
        {
            int result;
            if (_userId != 0)
            {
                result = _userId;
            }
            else
            {
                if (base.Session["uid"] != null)
                {
                    _userId = Convert.ToInt16(base.Session["uid"].ToString());
                    result = _userId;
                }
                else
                {
                    HttpCookie httpCookie = base.Request.Cookies["xnuid"];
                    if (httpCookie != null && httpCookie.Value != "")
                    {
                        short num = Convert.ToInt16(httpCookie.Value);
                        base.Session["uid"] = num;
                        base.Session.Timeout = 600;
                        _userId = num;
                        result = num;
                    }
                    else
                    {
                        result = -1;
                    }
                }
            }
            return result;
        }
        public void DataBaseInit()
        {
            using (var duoDb = new PortalDb())
            {
                //角色 
                if (!duoDb.Roles.Any())
                {
                    duoDb.Roles.Add(new UserRole
                    {
                        IsSystemRole = true,
                        Active = true,
                        Name = "系统管理员",
                        SystemName = SystemUserRoleNames.Administrators
                    });
                    duoDb.Roles.Add(new UserRole
                    {
                        IsSystemRole = true,
                        Active = true,
                        Name = "管理员",
                        SystemName = SystemUserRoleNames.Admin
                    });
                    duoDb.Roles.Add(new UserRole
                    {
                        IsSystemRole = true,
                        Active = true,
                        Name = "操作员",
                        SystemName = SystemUserRoleNames.Employeer
                    });

                }

                duoDb.SaveChanges();
            }
        }

        public FileResult ExportExcel(DataTable table, string path)
        {
            MemoryStream ms = ExcelRender.RenderToExcel(table);

            return File(ms, "application/vnd.ms-excel", path);
        }

        public void Success(string str = "操作成功")
        {
            TempData["msg"] = str;
        }

        public void Error(string str = "操作失败")
        {
            TempData["err"] = str;
        }

        protected virtual HttpCookie GetUserCookie()
        {
            return HttpContext.Request.Cookies[PortalConfig.UserCookieName];
        }

        protected virtual void SetUserCookie(Guid userGuid)
        {
            var cookie = new HttpCookie(PortalConfig.UserCookieName) { HttpOnly = true, Value = userGuid.ToString() };
            if (userGuid == Guid.Empty)
            {
                cookie.Expires = DateTime.Now.AddMonths(-1);
            }
            else
            {
                const int cookieExpires = 24 * 365; //TODO make configurable
                cookie.Expires = DateTime.Now.AddHours(cookieExpires);
            }

            HttpContext.Response.Cookies.Remove(PortalConfig.UserCookieName);
            HttpContext.Response.Cookies.Add(cookie);
        }

        public bool IsAdmin { get; set; }

        public void AsyncSendMail(string toEmails, string emailText, string subject)
        {
            Action action = () => SendMail.SendaMail(toEmails, emailText, subject);
            action.BeginInvoke(null, action);
        }

        public SendMail SendMail { get; set; }
    }
}
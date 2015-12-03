using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Niqiu.Core.Services;
using Portal.MVC.Attributes;
using Portal.MVC.Controllers;
using Portal.MVC.ViewModel;

namespace Portal.MVC.Areas.Admin.Controllers
{

    [LoginValid]
    public class UserController : BaseController
    {
        //
        // GET: /User/
        private readonly IUserService _service;

        public UserController(IUserService repository)
        {
            _service = repository;
        }


        [AdminAuthorize("ManageUsers")]
        public ActionResult Index()
        {
            var users = _service.GetAllUsers();
            return View(users);
        }

        //[Ninject.Inject]
        [AdminAuthorize("ManageUsers")]
        public ActionResult Create(int id = 0)
        {
            ViewBag.Des = "新增操作员";
            RegisterModel user;
            if (id == 0)
            {
               user = new RegisterModel(id);
            }
            else
            {
                var rawuser = _service.GetUserById(id);
                if (rawuser == null) return View("NoData");
                ViewBag.Des = "编辑用户";
                user=new RegisterModel(rawuser);
                Session["NameKey"] = rawuser.Username;
            }

            var roles = _service.GetAllUserRoles().TakeWhile(n=>n.SystemName!=SystemUserRoleNames.Administrators);
            ViewBag.Roles = new SelectList(roles, "Id", "Name"); 

            return View(user);
        }

        [HttpPost]
        public ActionResult Create(RegisterModel model)
        {
            ViewBag.Des = "新增操作员";
            var roles = _service.GetAllUserRoles().TakeWhile(n => n.SystemName != SystemUserRoleNames.Administrators);
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            User newuser;
            if (model.Id == 0)
            {
                var user = new User
                {
                    UserGuid = Guid.NewGuid(),
                    Username = model.UserName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Active = true,
                    //加密存储
                    Password = Encrypt.EncryptString(model.Password),
                };
                //默认增加注册角色
                // 先插入
                _service.InsertUser(user);
                newuser = _service.GetUserByUsername(user.Username);
            }
            else
            {
                newuser = _service.GetUserById(model.Id);
                newuser.Username = model.UserName;
                newuser.Password = Encrypt.EncryptString(model.Password);
                newuser.Email = model.Email;
                newuser.Mobile = model.Mobile;
                ViewBag.Des = "编辑用户";
            }

            var role = _service.GetUserRoleById(model.RoleId);
            //先只有一个角色
            newuser.UserRoles.Clear();
            newuser.UserRoles.Add(role);

            try
            {
                _service.UpdateUser(newuser);
                Success();
                model.Empty();
            }
            catch (Exception e)
            {
                Error(e.Message);
            }
            return View(model);
        }

        public JsonResult CheckUserName(string username)
        {
            if (Session["NameKey"] != null)
            {
                //说明正在编辑用户。
                if (string.IsNullOrWhiteSpace(username) || username.Length < 2)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                //不能和其他人同名。
                //说明不是本人
                if (Session["NameKey"].ToString() != username)
                {
                    var tag = _service.GetUserByUsername(username);
                    return Json(tag==null, JsonRequestBehavior.AllowGet);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var haSame = _service.GetUserByUsername(username);
            return Json(haSame == null, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportUser()
        {
            var res = _service.GetAllUsers();
            var len = res.Count;
            DataTable table = new DataTable();
            table.Columns.Add("登录名", typeof(string));
            table.Columns.Add("真实姓名", typeof(string));
            table.Columns.Add("角色", typeof(string));
            table.Columns.Add("手机号", typeof(string));
            table.Columns.Add("创建时间", typeof(DateTime));
            table.Columns.Add("最后登录时间", typeof(DateTime));
            for (int i = 0; i < len; i++)
            {
                table.Rows.Add(res[i].Username, res[i].RealName, res[i].RoleName(), res[i].Mobile, res[i].CreateTime, res[i].LastLoginDateUtc);
            }

            return ExportExcel(table, "用户.xls");
        }

        //
        // POST: /User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            User user = _service.GetUserById(id);
            _service.DeleteUser(user);
            return Json(id);
        }

        
    }
}
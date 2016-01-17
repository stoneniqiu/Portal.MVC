using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Niqiu.Core.Services;
using Portal.MVC.Attributes;
using Portal.MVC.ViewModel;

namespace Portal.MVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _portalContext;
        private const int PageSize = 5;
        private string ValidEmail;
        public UserController(IWorkContext portalContext, IUserService userService)
        {
            _portalContext = portalContext;
            _userService = userService;
        }
        #region 个人中心



        [LoginValid]
        [UserLastActivityIp]
        public ActionResult Index()
        {
            ViewBag.userName = _portalContext.CurrentUser.Username;
            var u = _portalContext.CurrentUser;
            return View(u);
        }

        [LoginValid]
        public ActionResult Avatar()
        {
            var u = _portalContext.CurrentUser;
            return View(u);
        }
        #endregion

        #region 修改密码
        [LoginValid]
        public ActionResult SetPassword()
        {
            ViewBag.userName = _portalContext.CurrentUser.Username;
            var model = new LocalPasswordModel();
            return View(model);
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult SetPassword(LocalPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _userService.GetUserById(_portalContext.CurrentUser.Id);
            if (!string.Equals(model.OldPassword.Trim(), Encrypt.GetMd5Code(user.Password)))
            {
                ModelState.AddModelError("OldPassword", "旧密码输入有误");
                return View(model);
            }
            user.Password = Encrypt.GetMd5Code(model.NewPassword.Trim());
            _userService.UpdateUser(user);
            return RedirectToAction("SetPasswordResult");
        }

        public ActionResult SetPasswordResult()
        {
            ViewBag.userName = _portalContext.CurrentUser.Username;
            return View();
        }


        #endregion

        #region 密码找回
        public ActionResult ForgotPwd()
        {
            Session.Timeout = 60;
            return View();
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionResult SendMail(string username = "")
        {
            if (Session["ValidEmail"] != null && !string.IsNullOrEmpty(Session["ValidEmail"].ToString()))
            {
                ViewBag.Email = Session["ValidEmail"].ToString();
                ViewBag.username = username;
                return View();
            }
            TempData["info"] = "验证码过期,请重新来";
            return RedirectToAction("ForgotPwd", "User");
        }
        /// <summary>
        /// 发送邮件成功
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionResult SendMailComplete(string username = "")
        {
            if (Session["ValidEmail"] == null || string.IsNullOrEmpty(Session["ValidEmail"].ToString()))
            {
                TempData["info"] = "验证码过期,请重新找回";
                return RedirectToAction("ForgotPwd", "User");
            }
            ViewBag.Email = Session["ValidEmail"].ToString();
            var timenow = DateTime.Now;
            var relative = Url.Action("ChangePassword", "User",
                new
                {
                    name = Encrypt.EncryptString(username),
                    active = Encrypt.EncryptString(DateTime.Now.ToString(CultureInfo.InvariantCulture))
                });
            if (Request.Url != null)
            {
                var url = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, "") + relative;

                var alink = string.Format("<a href='{0}'>{0}</a>", url);
                var content = string.Format("亲爱的用户 {0}: 您好！<br /> <br />您收到这封这封电子邮件是因为您 (也可能是某人冒充您的名义) 申请了一个新的密码。假如这不是您本人所申请, 请不用理会这封电子邮件, 但是如果您持续收到这类的信件骚扰, 请您尽快联络管理员。" +
                                            "<br /> 要使用新密码,请使用以下链接启用密码。<br />{1}(如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。该链接使用后将立即失效。)<br />" +
                                            "注意:请您在收到邮件1个小时内({2}前)使用，否则该链接将会失效。<br /><br />",
                    username, alink, timenow.AddHours(1));

                //send mail;
                try
                {
                    var res = _portalContext.SendMail(Session["ValidEmail"].ToString(), content, "找回密码");
                    if (!res)
                    {
                        TempData["error"] = "邮件发送失败,请确认邮箱是否正确";
                    }
                    else
                    {
                        //  AddFindPasswordLog(user.Id, user.UserName);
                    }
                }
                catch (Exception e)
                {

                    TempData["error"] = "邮件发送失败" + e.Message;
                }
            }
            return View();
        }

        /// <summary>
        /// 用户找回密码的时候修改密码
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="active">操作时间</param>
        /// <returns></returns>
        public ActionResult ChangePassword(string name, string active)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(active))
            {
                TempData["error"] = "链接已经失效";
                return RedirectToAction("ForgotPwd");
            }

            try
            {
                var username = Encrypt.DecryptString(name);
                var activetime = Encrypt.DecryptString(active);
                var time = Convert.ToDateTime(activetime);
                if (DateTime.Now > time.AddHours(1))
                // if (DateTime.Now > time.AddHours(1) || GetFindPasswordLog(name) == null)
                {
                    TempData["error"] = "链接已经失效";
                    return RedirectToAction("ForgotPwd");
                }

                ViewBag.Id = _userService.GetUserByUsername(username).Id;
            }
            catch
            {
                TempData["error"] = "时间或链接有误";
                return RedirectToAction("ForgotPwd");
            }

            return View();
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            var vCode = ImageManageHelper.CreateValidateCode(5);
            Session["ValidateCode"] = vCode;
            var aa = Session["ValidateCode"];
            byte[] bytes = ImageManageHelper.CreateValidateGraphic(vCode);
            return File(bytes, @"image/jpeg");
        }

        public ActionResult PartInfo()
        {
            User u = _portalContext.CurrentUser;
            return PartialView(u);
        }



        /// <summary>
        /// 改变头像
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAvatar(string url)
        {
            try
            {
                var user = _portalContext.CurrentUser;
                user.ImgUrl = url;
                _userService.UpdateUser(user);
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="pwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult ChangePassword(int id, string pwd)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                user.Password = Encrypt.GetMd5Code(pwd);
                _userService.UpdateUser(user);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 修改密码成功
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadImg(HttpPostedFileBase fileData)
        {
            if (CheckImg(fileData) != "ok") return Json(new { Success = false, Message = "文件格式不对！" }, JsonRequestBehavior.AllowGet);

            if (fileData != null)
            {
                var uploadpath = Server.MapPath("../Content/UploadFiles/");
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                //string saveName = Guid.NewGuid() + fileExtension; // 保存文件名称 这是个好方法。
                string saveName = Encrypt.GenerateOrderNumber() + fileExtension; // 保存文件名称 这是个好方法。
                fileData.SaveAs(uploadpath + saveName);

                return Json(new { Success = true, FileName = fileName, SaveName = saveName });
            }

            return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 核对图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string CheckImg(HttpPostedFileBase file)
        {
            if (file == null) return "图片不能空！";
            var extension = Path.GetExtension(file.FileName);
            if (extension != null)
            {
                var image = extension.ToLower();
                if (image != ".bmp" && image != ".png" && image != ".gif" && image != ".jpg" && image != ".jpeg")// 这里你自己加入其他图片格式，最好全部转化为大写再判断，我就偷懒了
                {
                    return "格式不对";
                }
            }
            return "ok";
        }


        #endregion


        #region 帮助程序
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        /// <summary>
        /// 检查是否有同名邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult CheckMail(string email)
        {
            var result = _userService.GetUserByEmail(email) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [LoginValid]
        public ActionResult ChangeMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeMail(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _portalContext.CurrentUser;
                user.Email = model.Email;
                _userService.UpdateUser(user);

                return RedirectToAction("SetEmail");
            }

            TempData["error"] = "修改失败";

            return View();
        }

        [LoginValid]
        public ActionResult SetEmail()
        {
            return View(_portalContext.CurrentUser);
        }

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public JsonResult CheckUserName(string username)
        {
            var result = _userService.GetUserByUsername(username) == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OutputCache(Duration = 0)]
        public JsonResult CheckvCode(string code)
        {
            if (Session["ValidateCode"] != null)
            {
                if (code == Session["ValidateCode"].ToString())
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                return Json(
                    "验证码错误", JsonRequestBehavior.AllowGet);
            }
            return Json("验证码已经过期", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取邮箱
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult GetEmail(string name, string code)
        {

            var user = _userService.GetUserByUsername(name);
            if (user == null)
                return Json("账户不存在", JsonRequestBehavior.AllowGet);

            if (Session["ValidateCode"] == null)
                return Json("验证码已过期", JsonRequestBehavior.AllowGet);

            if (Session["ValidateCode"].ToString() != code)
                return Json("验证码错误", JsonRequestBehavior.AllowGet);

            Session["ValidEmail"] = user.Email;
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}

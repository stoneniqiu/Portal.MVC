using System;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Helpers;
using Niqiu.Core.Services;
using Portal.MVC.ViewModel;

namespace Portal.MVC.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _service;
        private readonly IWorkContext _workContext;
        public AccountController(IUserService repository, IAccountService accountService,IWorkContext workContext)
        {
            _service = repository;
            _workContext = workContext;
            _accountService = accountService;
        }

        #region 修改密码

        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _workContext.CurrentUser;
                if (_accountService.ChangePassword(user.Id, model.Password))
                {
                    Success();
                }
                else
                {
                    Error();
                }
                return View();
            }
            Error();
            return View();
        }

        public JsonResult CheckPassword(string password)
        {
            var user = _workContext.CurrentUser;
            if (user != null)
            {
                var res= _accountService.ValidateUser(user.Username, password);
                return Json(res == UserLoginResults.Successful,JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 登陆退出

        [HttpGet]
        public ActionResult Logon(string returnUrl = "")
        {
            var model = new LogOnModel();
            ViewBag.ReturnUrl = returnUrl;
            //DataBaseInit();
            //if (!_service.GetAllUsers().Any())
            //{
            //    var user = new User
            //    {
            //        UserGuid = Guid.NewGuid(),
            //        Username = "stoneniqiu",
            //        RealName = "stoneniqiu",
            //        Mobile = "15250198031",
            //        Active = true,
            //        //加密存储
            //        Password = Encrypt.GetMd5Code("admin"),
            //    };
            //    var role = _service.GetUserRoleBySystemName(SystemUserRoleNames.Administrators);
            //    user.UserRoles.Add(role);
            //    //默认增加注册角色
            //    // 先插入
            //    _service.InsertUser(user);

            //}
            return View(model);
        }

        [HttpPost]
        public ActionResult Logon(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.UserName != null)
                {
                    model.UserName = model.UserName.Trim();
                }
                UserLoginResults loginResult = _accountService.ValidateUser(model.UserName, model.Password);

                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                    {
                        User user = _service.GetUserByUsername(model.UserName);
                        //sign in new customer
                        AuthenticationService.SignIn(user, model.RememberMe);

                        if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                            return RedirectToAction("Index", "Home");
                        return Redirect(returnUrl);
                    }
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", "用户不存在");
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", "用户已删除");
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", "用户没有激活");
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", "用户未注册");
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "密码错误");
                        break;
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        /// <summary>
        /// 注册【加密类型】
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            //如果当前用户再注册别的用户，就让他先退出，加入一个Guest角色用户进来准备。
            var user = _service.InsertGuestUser();

            if (ModelState.IsValid)
            {
                if (model.UserName != null)
                {
                    model.UserName = model.UserName.Trim();
                }

                var isApprove = true;
                var registerRequest = new UserRegistrationRequest(user, model.Email, model.Mobile, model.UserName, model.Password, PasswordFormat.Encrypted, isApprove);
                var registrationResult = _accountService.RegisterUser(registerRequest);
                if (registrationResult.Success)
                {
                    if (isApprove)
                    {
                        AuthenticationService.SignIn(user, true);
                    }
                    if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
                foreach (var error in registrationResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }

            }
            return View(model);
        }


        /// <summary>
        ///     退出函数 还需要处理，退出时统计退出时间,然后关闭网页。
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            AuthenticationService.SignOut();
            return RedirectToAction("Logon", "Account");
        }

        public ActionResult Unauthorized(string name, string returnUrl)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Logon", new { returnUrl });
            }
            ViewBag.P = name;
            return View();
        }



        #endregion


        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

    }
}
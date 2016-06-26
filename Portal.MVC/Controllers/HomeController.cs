using System;
using System.Linq;
using System.Web.Mvc;
using Niqiu.Core.Services;

namespace Portal.MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        public HomeController(IPermissionService permissionService,IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }


        public ActionResult Index()
        {
            _permissionService.InstallPermissions(new StandardPermissionProvider());
            //清除下多余的guest。

            // delete from users where Username is null and  LastActivityDateUtc<=DATEADD(DAY,-1,GETDATE()) //删除临时用户


            return View();
        }


        public ActionResult BookWay()
        {
            return View();
        }





    }
}

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Niqiu.Core.Services;
using Portal.MVC.Attributes;
using Portal.MVC.Controllers;

namespace Portal.MVC.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPermissionService _permissionService;
        private readonly IUserService _service;
        public HomeController(IUserService repository, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _service = repository;
        }

        [LoginValid]
        public ActionResult Index()
        {
            ViewBag.UserCount = _service.GetAllUsers().Count;
          //  _permissionService.InstallPermissions(new StandardPermissionProvider());
            return View();
        }
    }
}

using System.Web.Mvc;
using Niqiu.Core.Services;
using Portal.MVC.Attributes;

namespace Portal.MVC.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult BookWay()
        {
            return View();
        }


        public ActionResult BookList()
        {
            return View();
        }



    }
}

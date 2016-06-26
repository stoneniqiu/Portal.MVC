using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("IBabycrib", "IBabycrib", new
            {
                controller = "Product",
                action = "IBabycrib",
                id = UrlParameter.Optional,

            }, new[] { "Portal.MVC.Controllers" });

            routes.MapRoute("baby", "baby", new
            {
                controller = "Product",
                action = "IBabycrib",
                id = UrlParameter.Optional,

            }, new[] { "Portal.MVC.Controllers" });


            routes.MapRoute("WeiXin", "WeiXinAuth", new
            {
                controller = "ExternalAuthWeiXin",
                action = "Index",
                id = UrlParameter.Optional,

            }, new[] { "Portal.MVC.Controllers" });


             routes.MapRoute("AddProductToCart-Test", "addproducttocart/test/{productId}",
                new
            {
                controller = "ShoppingCart",
                action = "AddProductToCart",
                productId = @"\d+",
            }, new[] { "Portal.MVC.Controllers" });


             routes.MapRoute("AddProductToCart-Catelog", "addproducttocart/catelog/{productId}/{shoppingCartTypeId}/{quantity}/{customText}",
                 new
             {
                 controller = "ShoppingCart",
                 action = "AddProductToCart_Catalog",
                 productId = @"\d+",
                 shoppingCartTypeId = @"\d+",
                 quantity = @"\d+",
                 customText = @"^\w+"
             },
             new { productId = @"\d+", shoppingCartTypeId = @"\d+", quantity = @"\d+", customText = @"^\w+" },
              new[] { "Portal.MVC.Controllers" });

            routes.MapRoute("AddProductToCart-Details", "addproducttocart/details/{productId}/{shoppingCartTypeId}",
              new
              {
                  controller = "ShoppingCart",
                  action = "AddProductToCart_Details",
                  productId = @"\d+",
                  shoppingCartTypeId = @"\d+"
              },
              //new {  },
           new[] { "Portal.MVC.Controllers" });


            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional,

            }, new[] { "Portal.MVC.Controllers" });
        }
    }
}
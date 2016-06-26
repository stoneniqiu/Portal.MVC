using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Niqiu.Core;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.Config;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Services;
using Niqiu.Core.Services.Catalog;
using Niqiu.Core.Services.Orders;
using Portal.MVC;
using Portal.MVC.Models;
using Portal.MVC.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Portal.MVC
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }



        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDbContext>().To<PortalDb>().InSingletonScope();
            kernel.Bind<IWebHelper>().To<WebHelper>().InRequestScope();
            kernel.Bind<IWorkContext>().To<WebWorkContext>().InSingletonScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));//带泛型约束注入

           //kernel.Bind<ICacheManager>().To<MemoryCacheManager>().InSingletonScope().Named(PortalConfig.PortalCacheStatic);
           kernel.Bind<ICacheManager>().To<PerRequestCacheManager>().WhenInjectedInto<UserService>().InRequestScope().Named(PortalConfig.PortalCacheRequest);


            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IPermissionService>().To<PermissionService>();
            kernel.Bind<IAccountService>().To<AccoutService>();
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IShoppingCartService>().To<ShoppingCartService>();
            kernel.Bind<ICacheManager>().To<PerRequestCacheManager>();
         
        }        
    }
}

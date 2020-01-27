using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using System;
using System.Web;
using System.Web.Http;
using ScraperLinkedInServer.App_Start;
using ScraperLinkedInServer.Services.AccountService;
using ScraperLinkedInServer.Services.AccountService.Interfaces;
using WebActivatorEx;
using WebApiContrib.IoC.Ninject;
using ScraperLinkedInServer.Repositories.AccountRepository.Interfaces;
using ScraperLinkedInServer.Repositories.AccountRepository;
using ScraperLinkedInServer.Services.AdvanceSettingService.Interfaces;
using ScraperLinkedInServer.Services.AdvanceSettingService;
using ScraperLinkedInServer.Services.SettingService.Interfaces;
using ScraperLinkedInServer.Services.SettingService;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository.Interfaces;
using ScraperLinkedInServer.Repositories.SettingRepository.Interfaces;
using ScraperLinkedInServer.Repositories.AdvanceSettingRepository;
using ScraperLinkedInServer.Repositories.SettingRepository;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace ScraperLinkedInServer.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
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
            //Bind Services
            kernel.Bind<IAccountService>().To<AccountService>();
            kernel.Bind<IAdvanceSettingService>().To<AdvanceSettingService>();
            kernel.Bind<ISettingService>().To<SettingService>();

            //Bind repositories
            kernel.Bind<IAccountRepository>().To<AccountRepository>();
            kernel.Bind<IAdvanceSettingRepository>().To<AdvanceSettingRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
        }
    }
}
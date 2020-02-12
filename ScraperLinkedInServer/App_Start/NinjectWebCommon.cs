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
using ScraperLinkedInServer.Repositories.CompanyRepository.Interfaces;
using ScraperLinkedInServer.Repositories.CompanyRepository;
using ScraperLinkedInServer.Repositories.DebugLogRepository.Interfaces;
using ScraperLinkedInServer.Repositories.DebugLogRepository;
using ScraperLinkedInServer.Repositories.ProfileRepository;
using ScraperLinkedInServer.Repositories.ProfileRepository.Interfaces;
using ScraperLinkedInServer.Repositories.SuitableProfileRepository.Interfaces;
using ScraperLinkedInServer.Repositories.SuitableProfileRepository;
using ScraperLinkedInServer.Services.CompanyService.Interfaces;
using ScraperLinkedInServer.Services.CompanyService;
using ScraperLinkedInServer.Services.DebugLogService.Interfaces;
using ScraperLinkedInServer.Services.DebugLogService;
using ScraperLinkedInServer.Services.ProfileService.Interfaces;
using ScraperLinkedInServer.Services.ProfileService;
using ScraperLinkedInServer.Services.SuitableProfileService.Interfaces;
using ScraperLinkedInServer.Services.SuitableProfileService;
using ScraperLinkedInServer.Services.PaymentService.Interfaces;
using ScraperLinkedInServer.Services.PaymentService;
using ScraperLinkedInServer.Repositories.PaymentRepository;
using ScraperLinkedInServer.Repositories.PaymentRepository.Interfaces;

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
            kernel.Bind<ICompanyService>().To<CompanyService>();
            kernel.Bind<IDebugLogService>().To<DebugLogService>();
            kernel.Bind<IProfileService>().To<ProfileService>();
            kernel.Bind<ISettingService>().To<SettingService>();
            kernel.Bind<ISuitableProfileService>().To<SuitableProfileService>();
            kernel.Bind<IPaymentService>().To<PaymentService>();

            //Bind repositories
            kernel.Bind<IAccountRepository>().To<AccountRepository>();
            kernel.Bind<IAdvanceSettingRepository>().To<AdvanceSettingRepository>();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            kernel.Bind<IDebugLogRepository>().To<DebugLogRepository>();
            kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<ISuitableProfileRepository>().To<SuitableProfileRepository>();
            kernel.Bind<IPaymentRepository>().To<PaymentRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
        }
    }
}
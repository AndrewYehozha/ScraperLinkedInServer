using AutoMapper;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Request;
using ScraperLinkedInServer.Models.Types;

namespace ScraperLinkedInServer.ObjectMappers
{
    public class Mapper
    {
        private static IMapper _instance;

        public static IMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<RegistrationRequest, AccountViewModel>()
                           .ForMember(ci => ci.Id, opt => opt.Ignore())
                           .ForMember(ci => ci.IsDeleted, opt => opt.Ignore())
                           .ForMember(ci => ci.IsBlocked, opt => opt.Ignore());


                        cfg.CreateMap<Account, AccountViewModel>();

                        cfg.CreateMap<AccountViewModel, Account>();


                        cfg.CreateMap<SettingViewModel, Setting>()
                           .ForMember(c => c.ScraperStatusID, opt => opt.MapFrom(ce => (int)ce.ScraperStatus));

                        cfg.CreateMap<Setting, SettingViewModel>()
                           .ForMember(c => c.ScraperStatus, opt => opt.MapFrom(ce => (ScraperStatuses)ce.ScraperStatusID));
                        //cfg.CreateMap<CompanyImportViewModel, Company>()
                        //   .ForMember(c => c.LinkedInURL, opt => opt.MapFrom(ci => ci.LinkedIn))
                        //   .ForMember(c => c.OrganizationURL, opt => opt.MapFrom(ci => ci.OrganizationNameURL))
                        //   .ForMember(ci => ci.Industry, opt => opt.MapFrom(c => c.Categories))
                        //   .ForMember(ci => ci.AmountEmployees, opt => opt.MapFrom(c => c.NumberofEmployees));

                        //cfg.CreateMap<SuitableProfile, ResultViewModel>()
                        //   .ForMember(ci => ci.Number, opt => opt.MapFrom(ce => ce.Id))
                        //   .ForMember(ci => ci.LeadSource, opt => opt.Ignore());

                        //cfg.CreateMap<DebugLog, DebugLogViewModel>();
                        ////.ForMember(ci => ci.CreatedOn, opt => opt.MapFrom(ce => DateTime.Parse(ce.CreatedOn.ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("ru-Ru"))))
                        //;

                        //cfg.CreateMap<SettingsViewModel, Setting>()
                        //   .ForMember(c => c.IntervalType, opt => opt.MapFrom(ce => (int)ce.IntervalType))
                        //   .ForMember(c => c.ScraperStatusID, opt => opt.MapFrom(ce => (int)ce.ScraperStatus))
                        //   .ForMember(c => c.Localization, opt => opt.MapFrom(ce => (int)ce.Localization))
                        //   .ForMember(c => c.ScraperStatus, opt => opt.Ignore());
                    });
                    _instance = config.CreateMapper();
                }

                return _instance;
            }
        }
    }
}
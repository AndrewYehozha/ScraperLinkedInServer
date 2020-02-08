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


                        cfg.CreateMap<AdvanceSettingViewModel, AdvanceSetting>()
                           .ForMember(c => c.IntervalType, opt => opt.MapFrom(ce => (int)ce.IntervalType));
                        cfg.CreateMap<AdvanceSetting, AdvanceSettingViewModel>()
                           .ForMember(c => c.IntervalType, opt => opt.MapFrom(ce => (IntervalTypesSettings)ce.IntervalType));


                        cfg.CreateMap<CompanyViewModel, Company>();
                        cfg.CreateMap<Company, CompanyViewModel>();


                        cfg.CreateMap<DebugLogViewModel, DebugLog>();
                        cfg.CreateMap<DebugLog, DebugLogViewModel>();


                        cfg.CreateMap<SettingViewModel, Setting>()
                           .ForMember(c => c.ScraperStatusID, opt => opt.MapFrom(ce => (int)ce.ScraperStatus));
                        cfg.CreateMap<Setting, SettingViewModel>()
                           .ForMember(c => c.ScraperStatus, opt => opt.MapFrom(ce => (ScraperStatuses)ce.ScraperStatusID));


                        cfg.CreateMap<SuitableProfileViewModel, SuitableProfile>();
                        cfg.CreateMap<SuitableProfile, SuitableProfileViewModel>();
                    });
                    _instance = config.CreateMapper();
                }

                return _instance;
            }
        }
    }
}
using AutoMapper;
using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Database;
using ScraperLinkedInServer.Models.Request;
using Profile = ScraperLinkedInServer.Database.Profile;
using System.Collections.Generic;

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


                        cfg.CreateMap<AdvanceSettingsViewModel, AdvanceSetting>()
                           .ForMember(c => c.IntervalType, opt => opt.MapFrom(ce => (int)ce.IntervalType));
                        cfg.CreateMap<AdvanceSetting, AdvanceSettingsViewModel>()
                           .ForMember(c => c.IntervalType, opt => opt.MapFrom(ce => (Models.Types.IntervalType)ce.IntervalType));


                        cfg.CreateMap<CompanyViewModel, Company>()
                           .ForMember(c => c.ExecutionStatusID, opt => opt.MapFrom(ce => (int)ce.ExecutionStatus))
                           .ForMember(c => c.Account, opt => opt.Ignore())
                           .ForMember(c => c.Profiles, opt => opt.Ignore())
                           .ForMember(c => c.SuitableProfiles, opt => opt.Ignore())
                           .ForMember(c => c.ExecutionStatus, opt => opt.Ignore());
                        cfg.CreateMap<Company, CompanyViewModel>()
                           .ForMember(c => c.ExecutionStatus, opt => opt.MapFrom(ce => (Models.Types.ExecutionStatus)ce.ExecutionStatusID));


                        cfg.CreateMap<DebugLogViewModel, DebugLog>();
                        cfg.CreateMap<DebugLog, DebugLogViewModel>();


                        cfg.CreateMap<ProfileViewModel, Profile>()
                           .ForMember(c => c.ExecutionStatusID, opt => opt.MapFrom(ce => (int)ce.ExecutionStatus))
                           .ForMember(c => c.ProfileStatusID, opt => opt.MapFrom(ce => (int)ce.ProfileStatus))
                           .ForMember(c => c.Account, opt => opt.Ignore())
                           .ForMember(c => c.ExecutionStatus, opt => opt.Ignore())
                           .ForMember(c => c.ProfileStatus, opt => opt.Ignore());

                        cfg.CreateMap<Profile, ProfileViewModel>()
                           .ForMember(c => c.ExecutionStatus, opt => opt.MapFrom(ce => (Models.Types.ExecutionStatus)ce.ExecutionStatusID))
                           .ForMember(c => c.ProfileStatus, opt => opt.MapFrom(ce => (Models.Types.ProfileStatus)ce.ProfileStatusID))
                           .ForMember(c => c.OrganizationName, opt => opt.MapFrom(ce => ce.Company.OrganizationName));

                        cfg.CreateMap<Company, CompanyProfilesViewModel>()
                           .ForMember(c => c.ExecutionStatus, opt => opt.MapFrom(ce => (Models.Types.ExecutionStatus)ce.ExecutionStatusID))
                           .ForMember(c => c.ProfilesViewModel, opt => opt.MapFrom(ce => Mapper.Instance.Map<IEnumerable<Profile>, IEnumerable<ProfileViewModel>>(ce.Profiles)));

                        cfg.CreateMap<SettingsViewModel, Setting>()
                           .ForMember(c => c.ScraperStatusID, opt => opt.MapFrom(ce => (int)ce.ScraperStatus));
                        cfg.CreateMap<Setting, SettingsViewModel>()
                           .ForMember(c => c.ScraperStatus, opt => opt.MapFrom(ce => (Models.Types.ScraperStatus)ce.ScraperStatusID));


                        cfg.CreateMap<SuitableProfileViewModel, SuitableProfile>();
                        cfg.CreateMap<SuitableProfile, SuitableProfileViewModel>();


                        cfg.CreateMap<PaymentViewModel, Payment>();
                        cfg.CreateMap<Payment, PaymentViewModel>();
                    });
                    _instance = config.CreateMapper();
                }

                return _instance;
            }
        }
    }
}
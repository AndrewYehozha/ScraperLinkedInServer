using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Entities
{
    public class CompanyProfilesViewModel : CompanyViewModel
    {
        public IEnumerable<ProfileViewModel> ProfilesViewModel { get; set; }
    }
}
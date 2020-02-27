using ScraperLinkedInServer.Models.Entities;
using ScraperLinkedInServer.Models.Types;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Request
{
    public class ProfileRequest
    {
        public ProfileViewModel ProfileViewModel { get; set; }
    }

    public class ProfilesRequest
    {
        public IEnumerable<ProfileViewModel> ProfilesViewModel { get; set; }
    }

    public class UpdateProfileExecutionStatusRequest
    {
        public ExecutionStatus ExecutionStatus { get; set; }
        public int CompanyId { get; set; }
        public int AccountId { get; set; }
    }
}
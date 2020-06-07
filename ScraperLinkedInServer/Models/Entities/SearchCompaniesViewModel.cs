using ScraperLinkedInServer.Models.Types;

namespace ScraperLinkedInServer.Models.Entities
{
    public class SearchCompaniesViewModel
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string HeadquartersLocation { get; set; }
        public string Website { get; set; }
        public string Specialties { get; set; }
        public ExecutionStatus ExecutionStatus { get; set; }
        public string AmountEmployees { get; set; }
        public string DateCreated { get; set; }
    }
}
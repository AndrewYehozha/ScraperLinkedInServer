using CsvHelper.Configuration;
using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.CSVMap
{
    public class CompanyExportCSVMap : ClassMap<ExportCompaniesViewModel>
    {
        public CompanyExportCSVMap()
        {
            Map(m => m.OrganizationName).Name("Organization Name", "OrganizationName").Optional();
            Map(m => m.OrganizationURL).Name("Organization Name URL", "OrganizationURL").Optional();
            Map(m => m.Founders).Name("Founders", "Founders").Optional();
            Map(m => m.HeadquartersLocation).Name("Headquarters Location", "HeadquartersLocation").Optional();
            Map(m => m.Website).Name("Website", "Website").Optional();
            Map(m => m.LinkedInURL).Name("LinkedIn", "LinkedInURL").Optional();
            Map(m => m.Specialties).Optional();
            Map(m => m.DateCreatedFormat).Optional();
            Map(m => m.ExecutionStatus).Name("Execution Status", "ExecutionStatus").Optional();
            Map(m => m.Facebook).Name("Facebook", "Facebook").Optional();
            Map(m => m.Twitter).Name("Twitter", "Twitter").Optional();
            Map(m => m.PhoneNumber).Name("Phone Number", "PhoneNumber").Optional();
            Map(m => m.AmountEmployees).Name("Number of Employees", "AmountEmployees").Optional();
            Map(m => m.Industry).Name("Industries", "Industry").Optional();
        }
    }
}
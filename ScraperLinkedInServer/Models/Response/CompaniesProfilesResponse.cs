﻿using ScraperLinkedInServer.Models.Entities;
using System.Collections.Generic;

namespace ScraperLinkedInServer.Models.Response
{
    public class CompaniesProfilesResponse : BaseResponse
    {
        public IEnumerable<CompanyProfilesViewModel> CompanyProfilesViewModel { get; set; }
    }
}
﻿using ScraperLinkedInServer.Models.Entities;

namespace ScraperLinkedInServer.Models.Response
{
    public class AccountResponse
    {
        AccountViewModel AccountViewModel { get; set; }
    }

    public class AccountUpdateResponse
    {
        public string Message { get; set; }
    }
}
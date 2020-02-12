using System;
using System.ComponentModel.DataAnnotations;

namespace ScraperLinkedInServer.Models.Request
{
    public class AuthorizationRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class AuthorizationWindowsServiceRequest
    {
        public Guid Guid { get; set; }
    }
}
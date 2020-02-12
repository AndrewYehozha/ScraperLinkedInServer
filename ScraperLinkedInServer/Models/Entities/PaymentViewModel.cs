using System;

namespace ScraperLinkedInServer.Models.Entities
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public Guid Guide { get; set; }
        public int Validity { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public int AccountId { get; set; }
        public DateTime? PaymentOn { get; set; }
    }
}
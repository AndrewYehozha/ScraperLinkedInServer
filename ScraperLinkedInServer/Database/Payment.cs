//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScraperLinkedInServer.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public int Id { get; set; }
        public System.Guid Guide { get; set; }
        public int Validity { get; set; }
        public System.DateTime CreateOn { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public int AccountId { get; set; }
        public Nullable<System.DateTime> PaymentOn { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
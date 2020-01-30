﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ScraperLinkedInDBEntities : DbContext
    {
        public ScraperLinkedInDBEntities()
            : base("name=ScraperLinkedInDBEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<DebugLog> DebugLogs { get; set; }
        public virtual DbSet<ExecutionStatus> ExecutionStatuses { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileStatus> ProfileStatuses { get; set; }
        public virtual DbSet<ScraperStatus> ScraperStatuses { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SuitableProfile> SuitableProfiles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AdvanceSetting> AdvanceSettings { get; set; }
        public virtual DbSet<IntervalType> IntervalTypes { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data.Models.Client;
using DDI.Data.Models.Common;

namespace DDI.Data
{
	public class DomainContext : DbContext
	{
		#region Public Properties

		public virtual DbSet<Address> Addresses { get; set; }

		public virtual DbSet<AlternateId> AlternateIds { get; set; }

		public virtual DbSet<ClergyStatus> ClergyStatuses { get; set; }

		public virtual DbSet<ClergyType> ClergyTypes { get; set; }

		public virtual DbSet<Constituent> Constituents { get; set; }

		public virtual DbSet<ConstituentAddress> ConstituentAddresses { get; set; }

		public virtual DbSet<ConstituentAlternateId> ConstituentAlternateIds { get; set; }

		public virtual DbSet<ConstituentContactInfo> ConstituentContactInfo { get; set; }

		public virtual DbSet<ConstituentDBA> ConstituentDBAs { get; set; }

		public virtual DbSet<ConstituentEducation> ConstituentEducations { get; set; }        
		 
		public virtual DbSet<ConstituentStatus> ConstituentStatuses { get; set; }

		public virtual DbSet<ConstituentType> ConstituentTypes { get; set; }

		public virtual DbSet<Denomination> Denominations { get; set; }

		public virtual DbSet<DoingBusinessAs> DoingBusinessAs { get; set; }

		public virtual DbSet<Education> Educations { get; set; }

		public virtual DbSet<EducationLevel> EducationLevels { get; set; }
         
		public virtual DbSet<Ethnicity> Ethnicities { get; set; }

		public virtual DbSet<Gender> Genders { get; set; }

		public virtual DbSet<IncomeLevel> IncomeLevels { get; set; }

		public virtual DbSet<Language> Languages { get; set; }

		public virtual DbSet<LogEntry> LogEntries { get; set; }

		public virtual DbSet<PaymentPreference> PaymentPreferences { get; set; }

		public virtual DbSet<Prefix> Prefixes { get; set; }

		public virtual DbSet<Profession> Professions { get; set; }

		public virtual DbSet<State> States { get; set; }

		#endregion Public Properties

		#region Public Constructors

		public DomainContext()
			: base("name=DomainContext")
		{
		}

		#endregion Public Constructors
	}
}

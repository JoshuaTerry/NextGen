﻿using DDI.Shared.Enums.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ConstituentAddress"), Hateoas(RouteNames.ConstituentAddress)]
    public class ConstituentAddress : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? ConstituentId { get; set; }

        public bool IsPrimary { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int StartDay { get; set; }

        public int EndDay { get; set; }

        public ResidentType ResidentType { get; set; }

        public Guid? AddressTypeId { get; set; }
        
        [MaxLength(128)]
        [Index]
        public string DuplicateKey { get; set; }

        // Navigation Properties
        public Address Address { get; set; }

        public Constituent Constituent { get; set; }

        public AddressType AddressType { get; set; }

        public override string DisplayName
        {
            get
            {
                return AddressType?.Code ?? Id.ToString();
            }
        }

        #endregion Public Properties
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Attributes;
using DDI.Data.Models.Common;

namespace DDI.Data.Models.Client.CRM
{
    [Table("Address"), EntityType("NAAD")]
    public class Address : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(255)]
        public string AddressLine1 { get; set; }

        [MaxLength(255)]
        public string AddressLine2 { get; set; }

        [MaxLength(128)]
        public string City { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? CountyId { get; set; }

        [MaxLength(128)]
        public string PostalCode { get; set; }

        public Guid? StateId { get; set; }
        
        public int LegacyKey { get; set; } // Used for data conversion

        public Guid? Region1Id { get; set; }
        public Guid? Region2Id { get; set; }
        public Guid? Region3Id { get; set; }
        public Guid? Region4Id { get; set; }

        // Non-mapped properties
        [NotMapped]
        public Country Country
        {
            get
            {
                return CommonDataCache.GetCountry(CountryId);
            }
            set
            {
                CountryId = value.Id;
            }
        }

        [NotMapped]
        public State State
        {
            get
            {
                return CommonDataCache.GetState(StateId);
            }
            set
            {
                StateId = value.Id;
            }
        }


        [NotMapped]
        public County County
        {
            get
            {
                return CommonDataCache.GetCounty(CountyId);
            }
            set
            {
                CountyId = value.Id;
            }
        }

        // Navigation Properties
        public ICollection<ConstituentAddress> ConstituentAddresses { get; set; }

        public Region Region1 { get; set; }
        public Region Region2 { get; set; }
        public Region Region3 { get; set; }
        public Region Region4 { get; set; }

        #endregion Public Properties
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using DDI.Shared.Models.Client;
using DDI.Shared.Models.Common;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Address")]
    public class Address : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
                //TODO: 
                return null;
                //return CommonDataCache.GetCountry(CountryId);
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
                //TODO: 
                return null;
                //return CommonDataCache.GetState(StateId);
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
                //TODO: 
                return null;
                //return CommonDataCache.GetCounty(CountyId);
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

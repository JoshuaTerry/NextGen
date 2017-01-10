using DDI.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace DDI.Shared.Models.Client.CRM
{
    
	[Table("RegionArea")]
	public class RegionArea : EntityBase
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        
        public int Level { get; set; }

        public Guid? RegionId { get; set; }

        // Navigation Properties
        public Region Region { get; set; }

        public Guid? CountryId { get; set; }

        public Guid? StateId { get; set; }

        public Guid? CountyId { get; set; }

        [MaxLength(128)]
        public string City { get; set; }

        [MaxLength(128)]
        public string PostalCodeLow { get; set; }

        [MaxLength(128)]
        public string PostalCodeHigh { get; set; }

        public int Priority { get; set; }

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

        #endregion Public Properties

        #region Public Methods


        #endregion

    }

}

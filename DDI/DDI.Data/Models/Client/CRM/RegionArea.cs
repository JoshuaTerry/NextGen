using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Models.Common;

namespace DDI.Data.Models.Client.CRM
{
    
	[Table("RegionArea")]
	public class RegionArea : BaseEntity
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        
        public int Level { get; set; }

        public Guid? RegionId { get; set; }

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
                return CommonDataCache.GetCountry(CountryId);
            }
            set
            {
                CountryId = value?.Id;
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
                StateId = value?.Id;
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
                CountyId = value?.Id;
            }
        }

        #endregion Public Properties

        #region Public Methods


        #endregion

    }

}

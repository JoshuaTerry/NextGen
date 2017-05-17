using DDI.Shared.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DDI.Shared.Models.Client.CRM
{

    [Table("RegionArea")]
	public class RegionArea : AuditableEntityBase
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

        private Country _country = null;
        [NotMapped]
        [ForeignKey("CountryId")]
        public Country Country
        {
            get
            {
                return _country;
            }
            set
            {
                CountryId = value?.Id;
                _country = value;
            }
        }

        private State _state = null;
        [NotMapped]
        [ForeignKey("StateId")]
        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                StateId = value?.Id;
                _state = value;
            }
        }

        private County _county = null;
        [NotMapped]
        [ForeignKey("CountyId")]
        public County County
        {
            get
            {
                return _county;
            }
            set
            {
                CountyId = value?.Id;
                _county = value;
            }
        }

        #endregion Public Properties

        #region Public Methods


        #endregion

    }

}

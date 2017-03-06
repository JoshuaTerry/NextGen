using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    
	[Table("Region")]
	public class Region : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }


        public int Level { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public Guid? ParentRegionId { get; set; }

        public Region ParentRegion { get; set; }

        [InverseProperty(nameof(ParentRegion))]
        public ICollection<Region> ChildRegions { get; set; }

        [InverseProperty(nameof(Address.Region1))]
        public ICollection<Address> Region1Addresses { get; set; }

        [InverseProperty(nameof(Address.Region2))]
        public ICollection<Address> Region2Addresses { get; set; }

        [InverseProperty(nameof(Address.Region3))]
        public ICollection<Address> Region3Addresses { get; set; }

        [InverseProperty(nameof(Address.Region4))]
        public ICollection<Address> Region4Addresses { get; set; }

        public ICollection<RegionArea> RegionAreas { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

	    [NotMapped]
	    public int NextLevel => Level + 1;

	    #endregion

    }

}

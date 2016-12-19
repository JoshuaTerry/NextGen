using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    
	[Table("Region")]
	public class Region
	{
		#region Public Properties 
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        public int Level { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Guid? ParentRegionId { get; set; }

        // Navigation Properties
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

        #endregion Public Properties
    }

}

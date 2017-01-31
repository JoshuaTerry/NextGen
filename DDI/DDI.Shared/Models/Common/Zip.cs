using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;

namespace DDI.Shared.Models.Common
{
    [Table("Zip")]
    public class Zip : EntityBase
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index]
        [MaxLength(5)]
        public string ZipCode { get; set; }

        [Column(TypeName ="money")]
        public decimal CoordinateNS { get; set; }

        [Column(TypeName = "money")]
        public decimal CoordinateEW { get; set; }
        
        public Guid? CityId { get; set; }

        // Navigation Properties

        public City City { get; set; }

        public ICollection<ZipBranch> ZipBranches { get; set; }

        public ICollection<ZipStreet> ZipStreets { get; set; }
        [NotMapped]
        public override Guid? CreatedBy
        {
            get
            {
                return base.CreatedBy;
            }

            set
            {
                base.CreatedBy = value;
            }
        }

        [NotMapped]
        public override DateTime? CreatedOn
        {
            get
            {
                return base.CreatedOn;
            }

            set
            {
                base.CreatedOn = value;
            }
        }

        [NotMapped]
        public override Guid? LastModifiedBy
        {
            get
            {
                return base.LastModifiedBy;
            }

            set
            {
                base.LastModifiedBy = value;
            }
        }

        [NotMapped]
        public override DateTime? LastModifiedOn
        {
            get
            {
                return base.LastModifiedOn;
            }

            set
            {
                base.LastModifiedOn = value;
            }
        }
        #endregion

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return ZipCode;
            }
        }

        #endregion

    }
}

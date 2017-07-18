using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Common
{
    [Table("County")]
    public class County : EntityBase
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [MaxLength(128)]
        public string Description { get; set; }

        [Index]
        [MaxLength(5)]
        public string FIPSCode { get; set; }

        [MaxLength(4)]
        public string LegacyCode { get; set; }

        public Guid? StateId { get; set; }

        public int Population { get; set; }

        [Column(TypeName = "money")]
        public decimal PopulationPerSqaureMile { get; set; }

        [Column(TypeName = "money")]
        public decimal PopulationPercentageChange { get; set; }

        // Navigation Properties

        public State State { get; set; }

        public ICollection<City> Cities { get; set; }
        #endregion

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Description;
            }
        }

        #endregion


    }
}

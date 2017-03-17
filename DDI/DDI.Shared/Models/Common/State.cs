using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;

namespace DDI.Shared.Models.Common
{
    [Table("State")]
    public class State : EntityBase
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(4)]
        public string StateCode { get; set; }

        public string Description { get; set; }

        [MaxLength(2)]
        public string FIPSCode { get; set; }

        public Guid? CountryId { get; set; }

        // Navigation Properties
        
        public Country Country { get; set; }

        public ICollection<County> Counties { get; set; }

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

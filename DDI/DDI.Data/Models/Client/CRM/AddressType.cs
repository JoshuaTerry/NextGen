using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.CRM
{
    [Table("AddressType")]
    public class AddressType : BaseEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Address> Addresses { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

        #endregion


    }
}

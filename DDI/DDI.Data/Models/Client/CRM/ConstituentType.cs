using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.CRM
{
    [Table("ConstituentType")]
    public class ConstituentType : BaseEntity
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsRequired { get; set; }

        [MaxLength(16)]
        public string BaseType { get; set; }

        [MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string NameFormat { get; set; }

        [MaxLength(128)]
        public string SalutationFormal { get; set; }

        [MaxLength(128)]
        public string SalutationInformal { get; set; }

        #endregion Public Properties

        public ICollection<Tag> Tags { get; set; }

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Name;
            }
        }

        #endregion
    }
}

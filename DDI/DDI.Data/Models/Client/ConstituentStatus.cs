using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
    [Table("ConstituentStatus")]
    public class ConstituentStatus : BaseEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }


        [MaxLength(128)]
        public string Code { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        #endregion Public Properties
    }
}

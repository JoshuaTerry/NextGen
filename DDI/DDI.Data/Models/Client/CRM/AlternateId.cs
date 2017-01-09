using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Data.Models.Client.CRM
{
    [Table("AlternateId")]
    public class AlternateId : BaseEntity
    {
        #region Public Properties      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Constituent Constituent { get; set; }

        public Guid? ConstituentId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }
        #endregion Public Properties
    }
}

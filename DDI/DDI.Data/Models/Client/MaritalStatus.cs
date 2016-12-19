using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Data.Models.Client
{
    [Table("AlternateId")]
    public class MaritalStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }         
        [MaxLength(128)]
        public string Name { get; set; }
    }
}

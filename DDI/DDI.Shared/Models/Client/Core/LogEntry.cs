using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    /// <summary>
    /// Information about an event logged by the application.
    /// </summary>
    /// <remarks>
    /// The table name is set explicitly to prevent Entity Framework from creating tables based on
    /// the DbSet names in the context class ("LogEntries" instead of "LogEntry").
    /// </remarks>
    [Table("LogEntry")]
    public class LogEntry : AuditableEntityBase
    {
        #region Public Properties

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(2000)]
        public string Exception { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Level { get; set; }

        [Required]
        [MaxLength(255)]
        public string Logger { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Message { get; set; }

        [Required]
        [MaxLength(255)]
        public string Thread { get; set; }

        #endregion Public Properties
    }
}

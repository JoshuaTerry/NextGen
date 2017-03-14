using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
	[Table("Prefix")]
	public class Prefix : EntityBase, ICodeEntity
    {
        #region Public Properties 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string LabelPrefix { get; set; }

        [MaxLength(128)]
        public string LabelAbbreviation { get; set; }

        [MaxLength(128)]
        public string Salutation { get; set; }

        public bool IsActive { get; set; }

        public bool ShowOnline { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }

        public Guid? GenderId { get; set; }
        #endregion Public Properties

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

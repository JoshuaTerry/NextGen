﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("NoteCode")]
    public class NoteCode : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName => string.IsNullOrWhiteSpace(Name) ? Code : $"{Code}: {Name}";


        #endregion

    }
}

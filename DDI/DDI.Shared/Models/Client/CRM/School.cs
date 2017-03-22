﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("School")]
    public class School : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(16)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

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

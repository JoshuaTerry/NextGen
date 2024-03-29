﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ContactCategory")]
    public class ContactCategory : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(1)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string SectionTitle { get; set; }

        [MaxLength(128)]
        public string TextBoxLabel { get; set; }

        public bool IsActive { get; set; }

        public Guid? DefaultContactTypeId { get; set; }
        [ForeignKey("DefaultContactTypeId")]
        public ContactType DefaultContactType { get; set; }

        public ICollection<ContactType> ContactTypes { get; set; }

        #endregion Public Properties

        #region Constants

        public const string EMAIL = "E";
        public const string PHONE = "P";
        public const string WEB = "W";
        public const string SOCIAL = "S";
        public const string PERSON = "N";
        public const string OTHER = "O";

        #endregion

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

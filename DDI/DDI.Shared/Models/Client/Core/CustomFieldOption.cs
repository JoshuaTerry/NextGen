﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.Core
{
    [Table("CustomFieldOption")]
    public class CustomFieldOption : IEntity
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CustomFieldId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return string.Format("{0}: {1}", Code, Description);
            }
        }

        #endregion
    }
}

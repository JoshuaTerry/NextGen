using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ConstituentPicture")]
    public class ConstituentPicture : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid ConstituentId { get; set; }

        public Constituent Constituent { get; set; }

        public Guid FileId { get; set; }

        public FileStorage File { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName => string.Empty;

        #endregion
    }
}

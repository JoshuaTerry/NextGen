﻿using DDI.Shared.Models.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("AddressType")]
    public class AddressType : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Address> Addresses { get; set; }

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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public interface IObjectChange<TPrincipal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        long Id { get; set; }
        long ChangeSetId { get; set; }
        IChangeSet<TPrincipal> ChangeSet { get; set; }
        IEnumerable<IPropertyChange<TPrincipal>> PropertyChanges { get; }
        void Add(IPropertyChange<TPrincipal> propertyChange);
        string DisplayName { get; set; }
        string ChangeType { get; set; }
        string TypeName { get; set; }
        string ObjectReference { get; set; }        
    }
}

﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public interface IPropertyChange<TPrincipal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        long Id { get; set; }
        long ObjectChangeId { get; set; }
        IObjectChange<TPrincipal> ObjectChange { get; set; }
        string PropertyName { get; set; }
        Type PropertyType { get; set; }
        string PropertyTypeName { get; set; }
        string OriginalDisplayName { get; set; }
        string OriginalValue { get; set; }
        string NewValue { get; set; }
        string NewDisplayName { get; set; }
        string ChangeType { get; set; }
        bool IsManyToMany { get; set; }
        bool IsForeignKey { get; set; }
    }
}

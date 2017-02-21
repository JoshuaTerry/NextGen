﻿using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public class PropertyChange : IPropertyChange<DDIUser>
    {
        public string ChangeType { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ObjectChangeId { get; set; }
        public ObjectChange ObjectChange { get; set; }
        [MaxLength(128)]
        public string PropertyName { get; set; }
        [MaxLength(512)]
        public string OriginalValue { get; set; }
        [MaxLength(512)]
        public string Value { get; set; }
        IObjectChange<DDIUser> IPropertyChange<DDIUser>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }
        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.Core
{
    [Table("FileStorage")]
    public class FileStorage : AuditableEntityBase
    {
        public override Guid Id { get; set; }
        public string Name { get; set; }        
        [StringLength(256)]
        public string Extension { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
        
        [NotMapped]
        public override string DisplayName => Name;
    }
}

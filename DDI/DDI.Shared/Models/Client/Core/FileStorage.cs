using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("FileStorage")]
    public class FileStorage : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }        

        [MaxLength(8)]
        public string Extension { get; set; }

        public long Size { get; set; }

        public byte[] Data { get; set; }

        [MaxLength(64)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [NotMapped]
        public override string DisplayName => $"{Name}.{Extension}";
    }
}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GLAccountSelection")]
    public class GLAccountSelection : ICanTransmogrify
    {
        [Key, Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Id))]
        public Account Account { get; set; }
        public string AccountNumber { get; set; }
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
    }

}

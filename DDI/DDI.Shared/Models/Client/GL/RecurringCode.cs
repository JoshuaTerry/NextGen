using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class RecurringCode : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public const string MONTHLY = "MO";
        public const string ONE_TIME = "OT";
        public const string ON_HOLD = "OH";
        public const string WEEKLY = "WE";
        public const string BIWEEKLY = "BW";
        public const string DAILY = "DA";
        public const string BIMONTHLY = "BM";
        public const string QUARTERLY = "QU";
        public const string SEMIANNUALLY = "SA";
        public const string SEMIMONTHLY = "SM";
        public const string YEARLY = "YR";
        public const string PERIOD = "PR";
    }
}

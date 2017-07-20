
using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum IssuanceMethod
    {
        [Description("Send Note")]
        SendNote = 0,

        [Description("Book Entry")]
        BookEntry = 1,

        [Description("Safekeep Note")]
        SafekeepNote = 2
    }
}

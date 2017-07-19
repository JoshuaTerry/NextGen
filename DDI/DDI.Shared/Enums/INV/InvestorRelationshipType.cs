using System.ComponentModel;

namespace DDI.Shared.Enums.INV
{
    public enum InvestmentRelationshipType
    {
        [Description("Primary")]
        Primary = 0,

        [Description("Joint")]
        Joint = 1,

        [Description("Beneficiary")]
        Beneficiary = 2
    }
}


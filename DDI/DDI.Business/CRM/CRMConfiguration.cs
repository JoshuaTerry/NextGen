using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Data.Models.Client.CRM;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo;

namespace DDI.Business.CRM
{
    [ModuleTypeAttribute(ModuleType.CRM)]
    public class CRMConfiguration : BaseConfiguration
    {
        public bool UseRegionSecurity { get; set; }

        public bool ApplyDeceasedTag { get; set; }

        public List<AddressType> HomeAddressTypes { get; set; }
    }
}

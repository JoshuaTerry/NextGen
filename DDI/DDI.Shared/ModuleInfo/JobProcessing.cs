using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.JobProcessing)]
    public class JobProcessing : ModuleInfoBase
    {
        public override string Code => "JP";
        public override string Name => "Job Processing";
    }
}

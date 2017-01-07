using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.ProcessManagement)]
    public class ProcessManagement : ModuleInfoBase
    {
        public override string Code => "PM";
        public override string Name => "Process Management";
    }
}

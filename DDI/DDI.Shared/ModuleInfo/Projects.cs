using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.ProjectManagement)]
    public class Projects : ModuleInfoBase
    {
        public override string Code => "PR";
        public override string Name => "Project Management";
    }
}

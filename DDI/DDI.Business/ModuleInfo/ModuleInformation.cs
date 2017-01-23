using DDI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.ModuleInfo
{
    public class ModuleInformation 
    {

        #region Constructors 

        public ModuleInformation()
        {
            ChildModules = new List<ModuleInformation>();
        }

        #endregion

        #region Properties

        public List<ModuleInformation> ChildModules { get; set; }
        public ModuleInformation ParentModule { get; set; }

        public string Code { get; set;  }
        public string Name { get; set; }


        public bool IsRequired { get; set; }

        public bool CanDisburse { get; set; }

        public string CheckStubInvoiceLabel { get; set; } = "";
        public bool HasCustomFields { get; set; } = false;

        public ModuleType ModuleType{ get; set; }

        public ModuleType ParentModuleType { get; set; }

        #endregion

    }
}

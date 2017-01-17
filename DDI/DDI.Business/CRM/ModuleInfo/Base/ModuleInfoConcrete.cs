using DDI.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Business.CRM.ModuleInfo.Base
{
    public class ModuleInfoConcrete 
    {

        #region Private Fields

        ModuleTypeAttribute _moduleTypeAttribute;

        #endregion

        #region Constructors 

        public ModuleInfoConcrete()
        {
            ChildModules = new List<ModuleInfoConcrete>();
        }

        #endregion

        #region Properties

        public List<ModuleInfoConcrete> ChildModules { get; set; }
        public ModuleInfoConcrete ParentModule { get; set; }

        public string Code { get; set;  }
        public string Name { get; set; }


        public virtual bool IsRequired { get; set; }

        public virtual bool CanDisburse { get; set; }

        public virtual string CheckStubInvoiceLabel { get; set; } = "";
        public virtual bool HasCustomFields { get; set; } = false;

        public ModuleType ModuleType{ get; set; }

        public ModuleType ParentModuleType { get; set; }

        #endregion

    }
}

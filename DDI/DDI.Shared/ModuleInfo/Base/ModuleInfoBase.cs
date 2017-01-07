using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;

namespace DDI.Shared.ModuleInfo.Base
{
    public abstract class ModuleInfoBase
    {
        #region Private Fields

        ModuleTypeAttribute _moduleTypeAttribute;

        #endregion

        #region Constructors 

        public ModuleInfoBase()
        {
            ChildModules = new List<ModuleInfoBase>();
        }

        #endregion

        #region Properties

        public List<ModuleInfoBase> ChildModules { get; set; }
        public ModuleInfoBase ParentModule { get; set; }

        public abstract string Code { get; }
        public abstract string Name { get; }

        public virtual bool IsRequired { get; } = false;

        public virtual bool CanDisburse { get; } = false;
        public virtual string CheckStubInvoiceLabel { get; } = "";
        public virtual bool HasCustomFields { get; } = false;

        public ModuleType ModuleType
        {
            get
            {
                if (_moduleTypeAttribute == null)
                {
                    _moduleTypeAttribute = this.GetType().GetAttribute<ModuleTypeAttribute>();
                }

                return _moduleTypeAttribute?.ModuleType ?? ModuleType.None;
            }
        }

        public ModuleType ParentModuleType
        {
            get
            {
                if (_moduleTypeAttribute == null)
                {
                    ModuleTypeAttribute attr = this.GetType().GetAttribute<ModuleTypeAttribute>();
                }

                return _moduleTypeAttribute?.ParentModuleType ?? ModuleType.None;
            }
        }

        #endregion

    }
}

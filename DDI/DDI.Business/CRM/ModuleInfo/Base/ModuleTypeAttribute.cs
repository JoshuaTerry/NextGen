using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;

namespace DDI.Business.CRM.ModuleInfo.Base
{
    /// <summary>
    /// Attribute for specifying ModuleType with a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleTypeAttribute : Attribute
    {
        #region Constructors

        public ModuleTypeAttribute(ModuleType moduleType)
        {
            ModuleType = moduleType;
        }

        public ModuleTypeAttribute(ModuleType moduleType, ModuleType parentType) : this(moduleType)
        {
            ParentModuleType = parentType;
        }

        #endregion

        #region Properties

        [Required]
        public ModuleType ModuleType
        {
            get;
            private set;
        }

        public ModuleType ParentModuleType
        {
            get;
            private set;
        }

        #endregion
    }
}

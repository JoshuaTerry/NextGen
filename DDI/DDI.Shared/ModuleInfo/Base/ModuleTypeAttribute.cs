using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;

namespace DDI.Shared.ModuleInfo
{
    /// <summary>
    /// Attribute for specifying ModuleType with a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleTypeAttribute : Attribute
    {
        #region Constructors

        public ModuleTypeAttribute(ModuleType modType)
        {
            ModuleType = modType;
        }

        public ModuleTypeAttribute(ModuleType modType, ModuleType parentType) : this(modType)
        {
            ParentModuleType = parentType;
        }


        #endregion Constructors

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

        #endregion Properties
    }
}

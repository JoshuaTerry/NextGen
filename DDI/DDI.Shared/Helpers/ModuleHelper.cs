using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.Helpers
{
    /// <summary>
    /// Methods for accessing module information.
    /// </summary>
    public static class ModuleHelper
    {
        #region Fields

        private static Dictionary<ModuleType, ModuleInfoBase> _moduleDict;
        private static List<ModuleInfoBase> _modules;

        #endregion

        #region Constructors

        static ModuleHelper()
        {
            _modules = new List<ModuleInfoBase>();
            _moduleDict = new Dictionary<ModuleType, ModuleInfoBase>();

            // Populate the list of modules and the module dictionary.
            foreach (Type moduleType in ReflectionHelper.GetDerivedTypes<ModuleInfoBase>(typeof(ModuleHelper).Assembly))
            {
                ModuleInfoBase mod = (ModuleInfoBase)Activator.CreateInstance(moduleType);
                _modules.Add(mod);
                ModuleType modType = mod.ModuleType;
                if (modType != ModuleType.None)
                {
                    _moduleDict[modType] = mod;
                }
            }            

            // Link up modules
            foreach (var mod in _modules)
            {
                ModuleType parent = mod.ParentModuleType;
                if (parent != ModuleType.None)
                {
                    ModuleInfoBase parentMod = _modules.FirstOrDefault(p => p.ModuleType == parent);
                    if (parentMod != null)
                    {
                        mod.ParentModule = parentMod;
                        parentMod.ChildModules.Add(mod);
                    }
                }
            }

        }

        #endregion

        #region Properties

        /// <summary>
        /// Collection of module information objects
        /// </summary>
        public static ICollection<ModuleInfoBase> ModuleInfoCollection
        {
            get
            {
                return _modules;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the ModuleInfo object for a specified ModuleType value.
        /// </summary>
        public static ModuleInfoBase GetModuleInfo (ModuleType modType)
        {
            ModuleInfoBase mod = null;
            _moduleDict.TryGetValue(modType, out mod);

            return mod;
        }

        /// <summary>
        /// Get the ModuleInfo object for a specified type.
        /// </summary>
        /// <typeparam name="T">ModuleInfo type</typeparam>
        public static T GetModuleInfo<T>() where T : ModuleInfoBase
        {
            return _modules.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }

        #endregion


    }
}

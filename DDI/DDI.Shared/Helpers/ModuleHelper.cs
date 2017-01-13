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

        private static Dictionary<ModuleType, ModuleInfoBase> _moduleDictionary;
        private static List<ModuleInfoBase> _modules;

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of module information objects
        /// </summary>
        public static ICollection<ModuleInfoBase> ModuleInfoCollection
        {
            get
            {
                Initialize();
                return _modules;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the ModuleInfo object for a specified ModuleType value.
        /// </summary>
        public static ModuleInfoBase GetModuleInfo (ModuleType moduleType)
        {
            Initialize();
            ModuleInfoBase moduleInfo = null;
            _moduleDictionary.TryGetValue(moduleType, out moduleInfo);

            return moduleInfo;
        }

        /// <summary>
        /// Get the ModuleInfo object for a specified type.
        /// </summary>
        /// <typeparam name="T">ModuleInfo type</typeparam>
        public static T GetModuleInfo<T>() where T : ModuleInfoBase
        {
            Initialize();
            return _modules.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build list of modules via reflection only once, and only when first requested.
        /// </summary>
        private static void Initialize()
        {
            if (_modules == null)
            {
                _modules = new List<ModuleInfoBase>();
                _moduleDictionary = new Dictionary<ModuleType, ModuleInfoBase>();

                // Populate the list of modules and the module dictionary.
                foreach (Type type in ReflectionHelper.GetDerivedTypes<ModuleInfoBase>(typeof(ModuleHelper).Assembly))
                {
                    ModuleInfoBase moduleInfo = (ModuleInfoBase)Activator.CreateInstance(type);
                    _modules.Add(moduleInfo);
                    ModuleType moduleType = moduleInfo.ModuleType;
                    if (moduleType != ModuleType.None)
                    {
                        _moduleDictionary[moduleType] = moduleInfo;
                    }
                }

                // Link up modules
                foreach (var module in _modules)
                {
                    ModuleType parent = module.ParentModuleType;
                    if (parent != ModuleType.None)
                    {
                        ModuleInfoBase parentModule = _modules.FirstOrDefault(p => p.ModuleType == parent);
                        if (parentModule != null)
                        {
                            module.ParentModule = parentModule;
                            parentModule.ChildModules.Add(module);
                        }
                    }
                }
            }
        }

        #endregion




    }
}

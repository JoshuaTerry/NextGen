using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Core;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Models;

namespace DDI.Business.Core
{
    public class ConfigurationLogic : EntityLogicBase<Configuration>
    {
        #region Private Fields

        // Cache settings
        private const int CONFIGURATION_TIMEOUT_SECS = 300; // Cached items removed after 5 minutes
        private const string CONFIGURATION_KEY = "CONFIG";

        private List<ConfigurationBase> _attachedConfigurations;

        #endregion

        #region Constructors 

        public ConfigurationLogic() : this(new UnitOfWorkEF()) { }

        public ConfigurationLogic(IUnitOfWork uow) : base(uow)
        {
            _attachedConfigurations = new List<ConfigurationBase>();
        }

        #endregion

        #region Public Methods

        public T GetConfiguration<T> (bool reload = false) where T : ConfigurationBase
        {
            T config = null;       
            
            string key = GetCacheKey<T>();
            if (reload)
            {
                CacheHelper.RemoveEntry(key);
            }
            // Retrieve config from cache
            config = CacheHelper.GetEntry(key, CONFIGURATION_TIMEOUT_SECS, false, () => LoadConfiguration(typeof(T)) as T, CacheItemRemoved);
            
            
            if (config != null && !_attachedConfigurations.Contains(config))
            {
                // If loaded from the cache, attach it to the unit of work.
                config.Attach(UnitOfWork);
                _attachedConfigurations.Add(config);
            }

            return config;
        }


        public void SaveConfiguration<T>(T config, bool saveChanges = true) where T : ConfigurationBase
        {
            if (config == null)
            {
                return;
            }

            // Save config to database.
            SaveConfigurationToDb(config);
            if (saveChanges)
            {
                UnitOfWork.SaveChanges();
            }

            string key = GetCacheKey<T>();

            // Update the cache
            CacheHelper.SetEntry(key, config, CONFIGURATION_TIMEOUT_SECS, false, CacheItemRemoved);

            if (!_attachedConfigurations.Contains(config))
            {
                _attachedConfigurations.Add(config);
            }            
        }

        #endregion

        #region Private Methods

        private string GetCacheKey<T>() where T : ConfigurationBase
        {
            return CONFIGURATION_KEY + typeof(T).Name;
        }

        private void SaveConfigurationToDb(ConfigurationBase config)
        {
            Type configType = config.GetType();
            Type EntityBaseType = typeof(EntityBase);

            ModuleType modType = config.ModuleType;

            // Load the entire set of config rows
            var configRows = UnitOfWork.Where<Configuration>(p => p.ModuleType == modType).ToList();

            // Iterate through each property
            foreach (var prop in configType.GetProperties())
            {
                bool failed = false;
                string valString = string.Empty;
                Type propType = prop.PropertyType;

                if (propType == typeof(string))
                {
                    // String properties
                    valString = prop.GetValue(config) as string;
                }
                else if (propType.IsEnum)
                {
                    // Enum properties
                    valString = ((int)(prop.GetValue(config))).ToString();
                }                
                else if (propType.IsPrimitive || propType.IsValueType)
                {
                    // Primitive and value types
                    var converter = TypeDescriptor.GetConverter(propType);                    
                    try
                    {
                        valString = converter.ConvertToInvariantString(prop.GetValue(config));
                    }
                    catch
                    {
                        failed = true;
                    }
                }
                else if (propType.IsSubclassOf(EntityBaseType))
                {
                    // Entity properties
                    var entity = prop.GetValue(config) as EntityBase;
                    if (entity != null)
                    {
                        // Convert id to string
                        valString = entity.Id.ToString();
                    }
                }
                else
                {
                    failed = true;
                }

                if (failed)
                {
                    // Try calling the configuration's SaveProperty override.
                    valString = config.SaveProperty(prop.Name);
                    if (valString != null)
                    {
                        failed = false;
                    }
                }

                if (!failed)
                {
                    Configuration setting = configRows.FirstOrDefault(p => p.Name == prop.Name);
                    if (setting == null)
                    {
                        setting = UnitOfWork.Create<Configuration>();
                        setting.ModuleType = modType;
                        setting.Name = prop.Name;
                    }
                    else
                    {
                        // Remove this setting row once it's been processed.
                        configRows.Remove(setting);
                    }
                    setting.Value = valString;
                }
            }

            // Delete any rows that weren't processed.
            foreach (var itemToDelete in configRows)
            {
                UnitOfWork.Delete(itemToDelete);
            }

            UnitOfWork.SaveChanges();
        }

        private ConfigurationBase LoadConfiguration(Type type)
        {
            //ModuleTypeAttribute attr = type.GetAttribute<ModuleTypeAttribute>();
            //if (attr == null)
            //{
            //    return null;
            //}

            Type EntityBaseType = typeof(EntityBase);

            // Create an instance of the config class.
            var config = (ConfigurationBase)Activator.CreateInstance(type);            

            // Iterate through each ModuleSetting and populate the config object.
            foreach (var row in UnitOfWork.Where<Configuration>(p => p.ModuleType == config.ModuleType))
            {
                string valString = row.Value;
                bool failed = false;

                // See if there's a property of this name.
                var prop = type.GetProperty(row.Name);
                if (prop != null)
                {
                    Type propType = prop.PropertyType;

                    if (propType == typeof(string))
                    {
                        // String properties
                        prop.SetValue(config, valString);
                    }
                    else if (propType.IsEnum)
                    {
                        // Enum properties - use EnumConverter
                        if (!string.IsNullOrWhiteSpace(valString))
                        {
                            try
                            {
                                var converter = new EnumConverter(propType);
                                prop.SetValue(config, converter.ConvertFromInvariantString(valString));                                
                            }
                            catch
                            {
                                failed = true;
                            }
                        }
                    }
                    else if (propType.IsPrimitive || propType.IsValueType)
                    {
                        // Value type properties - use a generic converter
                        try
                        {
                            if (propType == typeof(DateTime?) && string.IsNullOrWhiteSpace(valString))
                            {
                                // DateTime? from blank should be null
                                prop.SetValue(config, null);
                            }
                            else
                            {
                                var converter = TypeDescriptor.GetConverter(propType);
                                prop.SetValue(config, converter.ConvertFromString(valString));
                            }
                        }
                        catch
                        {
                            failed = true;
                        }
                    }
                    else if (propType.IsSubclassOf(EntityBaseType))
                    {
                        // Entity properties - Use reflection to call BaseConfiguration.GetEntity<T>
                        try
                        {
                            MethodInfo generic = typeof(ConfigurationBase).GetMethod(nameof(ConfigurationBase.GetEntity)).MakeGenericMethod(propType);
                            prop.SetValue(config, generic.Invoke(config, new object[] { valString, UnitOfWork }));
                        }
                        catch
                        {
                            failed = true;
                        }
                    }
                    else
                    {
                        failed = true; // Property not convertible.
                    }               
                }
                else
                {
                    failed = true; // Could not find matching property.
                }

                if (failed)
                {
                    // Try calling the configuration's LoadProperty override.
                    config.LoadProperty(row.Name, row.Value, UnitOfWork);
                }

            }

            return config;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Callback for when a cached configuration is removed.  This will remove it from _attachedConfigurations.
        /// </summary>
        /// <param name="arguments"></param>
        private void CacheItemRemoved(ConfigurationBase config)
        {        
            if (config != null)
            {
                _attachedConfigurations.Remove(config);
            }
        }


        #endregion

    }
}

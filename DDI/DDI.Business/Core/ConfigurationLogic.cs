using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Models.Client.Core;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.ModuleInfo;

namespace DDI.Business.Core
{
    public class ConfigurationLogic : BaseEntityLogic<Configuration>
    {
        #region Private Fields


        #endregion

        #region Constructors 

        public ConfigurationLogic() : this(new UnitOfWorkEF()) { }

        public ConfigurationLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Methods

        public T GetConfiguration<T> () where T : BaseConfiguration
        {



            return null;
        }
        #endregion

        #region Private Methods

        private void SaveConfiguration(BaseConfiguration config)
        {
            Type configType = config.GetType();

            ModuleTypeAttribute attr = configType.GetAttribute<ModuleTypeAttribute>();
            if (attr == null)
            {
                return;
            }

            ModuleType modType = attr.ModuleType;

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
                    valString = prop.GetValue(config) as string;
                }
                else if (propType.IsEnum)
                {
                    valString = ((int)(prop.GetValue(config))).ToString();
                }
                else if (propType.IsPrimitive || propType.IsValueType)
                {
                    var converter = TypeDescriptor.GetConverter(propType);
                    try
                    {
                        valString = converter.ConvertToString(prop.GetValue(config));
                    }
                    catch
                    {
                        failed = true;
                    }
                }
                else
                {
                    failed = true;
                }

                if (failed)
                {
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
                        configRows.Remove(setting);
                    }
                    setting.Value = valString;
                }
            }

            foreach (var itemToDelete in configRows)
            {
                UnitOfWork.Delete(itemToDelete);
            }

            UnitOfWork.SaveChanges();
        }

        private BaseConfiguration LoadConfiguration(Type type)
        {
            ModuleTypeAttribute attr = type.GetAttribute<ModuleTypeAttribute>();
            if (attr == null)
            {
                return null;
            }

            ModuleType modType = attr.ModuleType;

            // Create an instance of the config class.
            var config = (BaseConfiguration)Activator.CreateInstance(type);            

            // Iterate through each ModuleSetting and populate the config object.
            foreach (var row in UnitOfWork.Where<Configuration>(p => p.ModuleType == modType))
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
                        prop.SetValue(config, valString);
                    }
                    else if (propType.IsEnum)
                    {
                        // Enum - use EnumConverter
                        if (!string.IsNullOrWhiteSpace(valString))
                        {
                            try
                            {
                                var converter = new EnumConverter(propType);
                                prop.SetValue(config, converter.ConvertFrom(valString));                                
                            }
                            catch
                            {
                                failed = true;
                            }
                        }
                    }
                    else if (propType.IsPrimitive || propType.IsValueType)
                    {
                        // Value types - use a generic converter
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
                    config.LoadProperty(row.Name, row.Value);
                }

            }

            return config;
        }

        #endregion

    }
}

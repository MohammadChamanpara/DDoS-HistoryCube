using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Library.Settings
{
    public class SettingService
    {
        #region Configuration File Names
        public static class ConfigurationFileNames
        {
            public static string CurrentAssemblyConfigFile
            {
                get
                {
                    return CustomAssemblyConfigFile(Assembly.GetCallingAssembly());
                }
            }
            public static string CurrentApplicationConfigFile
            {
                get
                {
                    return CustomAssemblyConfigFile(Assembly.GetEntryAssembly());
                }
            }
            public static string CustomAssemblyConfigFile(Type aTypeInDLL)
            {
                return CustomAssemblyConfigFile(aTypeInDLL.Assembly);
            }
            public static string CustomAssemblyConfigFile(Assembly assembly)
            {
                return assembly.Location + DefaultConfigurationExtension;
            }
            internal static string DefaultConfigurationFileName
            {
                get
                {
                    return CurrentApplicationConfigFile;
                }
            }
            public static string DefaultConfigurationSectionName
            {
                get
                {
                    return "ApplicationSettings";
                }
            }
            private static string DefaultConfigurationExtension
            {
                get
                {
                    return ".config";
                }
            }
        }
        #endregion

        #region Variables
        private ConfigurationSectionHandler configurationSectionHandler;
        private object settingContainerObject;
        private Type settingContainerType;
        #endregion

        #region Constructor
        public SettingService(string configurationFileName, string configurationSectionName, Type settingContainerType)
            : this
                (
                configurationFileName,
                configurationSectionName,
                null,
                settingContainerType
                )
        { }

        public SettingService(string configurationFileName, string configurationSectionName, object settingContainerObject) :
            this
             (
                 configurationFileName,
                 configurationSectionName,
                 settingContainerObject,
                 settingContainerObject.GetType()
             )
        { }

        private SettingService(string configurationFileName, string configurationSectionName, object settingContainerObject, Type settingContainerType)
        {
            this.settingContainerObject = settingContainerObject;
            this.settingContainerType = settingContainerType;
            this.configurationSectionHandler = new ConfigurationSectionHandler(configurationFileName, configurationSectionName);
        }

        #endregion

        #region Properties
        public string SettingFilePath
        {
            get
            {
                if (this.configurationSectionHandler == null)
                    return "";
                return this.configurationSectionHandler.ConfigurationFileName;
            }
        }
        #endregion

        #region Methods
        private T LoadSetting<T>(string key, T defaultValue, Type propertyType)
        {
            return configurationSectionHandler.LoadSetting(key, defaultValue, propertyType);
        }
        private void SaveSetting(string key, object newValue)
        {
            configurationSectionHandler.SaveSetting(key, newValue);
        }

        public void LoadSettings()
        {
            foreach (PropertyInfo property in settingContainerType.GetProperties())
                foreach (var attribute in property.GetCustomAttributes(false))
                    if (attribute is SettingPropertyAttribute)
                    {
                        var defaultValue = (attribute as SettingPropertyAttribute).DefaultValue;

                        if (defaultValue != null)
                        {
                            if (!property.PropertyType.IsAssignableFrom(defaultValue.GetType()))

                                throw HelperMethods.CreateException("نوع مقدار پیش فرض برای {0} برابر با {1} است ولی نوع مورد قبول {2} می باشد.",
                                    property.Name, defaultValue.GetType(), property.PropertyType);
                        }

                        property.SetValue
                        (
                            settingContainerObject,
                            this.LoadSetting(property.Name, defaultValue, property.PropertyType),
                            null
                        );
                    }
        }
        public void SaveSettings()
        {
            foreach (PropertyInfo property in settingContainerType.GetProperties())
                foreach (var attribute in property.GetCustomAttributes(false))
                    if (attribute is SettingPropertyAttribute)
                        this.SaveSetting(property.Name, property.GetValue(settingContainerObject, null));
        }

        private void CheckAttribute(string propertyName, PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof(SettingPropertyAttribute), false).Length == 0)
                throw HelperMethods.CreateException("{0} property is not decorated with {1}.", propertyName, typeof(SettingPropertyAttribute).Name);
        }
        private PropertyInfo FindProperty(string propertyName)
        {
            PropertyInfo property = settingContainerType.GetProperty(propertyName);
            if (property == null)
                throw HelperMethods.CreateException("Property with name \'{0}\' does not exist.", propertyName);
            return property;
        }

        #endregion

        #region Indexers
        public object this[string propertyName]
        {
            get
            {
                return FindProperty(propertyName).GetValue(settingContainerObject, null);
            }
            set
            {
                FindProperty(propertyName).SetValue(settingContainerObject, value, null);
            }
        }
        #endregion
    }
}

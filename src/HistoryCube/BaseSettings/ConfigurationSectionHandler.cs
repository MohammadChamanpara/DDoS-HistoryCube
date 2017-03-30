using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using Library.Log;

namespace Library.Settings
{
    public class ConfigurationSectionHandler : ConfigurationSection
    {
        #region Variables
        private Configuration configuration;
        private ConfigurationSectionHandler configurationSection;
        private KeyValueConfigurationCollection collection;
        #endregion

        #region Constructors

        public ConfigurationSectionHandler() :
            this
            (
                SettingService.ConfigurationFileNames.DefaultConfigurationFileName,
                SettingService.ConfigurationFileNames.DefaultConfigurationSectionName
            ) { }

        public ConfigurationSectionHandler(string configurationFileName, string configurationSectionName)
        {
            this.ConfigurationFileName = configurationFileName;
            this.ConfigurationSectionName = configurationSectionName;
        }

        #endregion

        #region Properties
        public string ConfigurationFileName { get; private set; }

        public string ConfigurationSectionName { get; private set; }

        private Configuration Configuration
        {
            get
            {
                if (this.configuration == null)
                {
                    HelperMethods.RepairXml(this.ConfigurationFileName);
                    this.configuration = ConfigurationManager.OpenMappedExeConfiguration
                    (
                        new ExeConfigurationFileMap { ExeConfigFilename = this.ConfigurationFileName },
                        ConfigurationUserLevel.None
                    );
                }
                return this.configuration;
            }
        }
        private ConfigurationSectionHandler ConfigurationSection
        {
            get
            {
                if (this.configurationSection == null)
                {
                    ConfigurationSection section = this.Configuration.GetSection(this.ConfigurationSectionName);
                    if (section == null)
                    {
                        this.Configuration.Sections.Add(this.ConfigurationSectionName, new ConfigurationSectionHandler(this.ConfigurationFileName, ConfigurationSectionName));
                        section = this.Configuration.GetSection(this.ConfigurationSectionName);
                    }
                    this.configurationSection = (ConfigurationSectionHandler)section;
                }
                return this.configurationSection;
            }
        }
        [ConfigurationProperty("", IsDefaultCollection = true)]
        private KeyValueConfigurationCollection Collection
        {
            get
            {
                if (this.collection == null)
                {
                    this.collection = (KeyValueConfigurationCollection)this.ConfigurationSection[""];
                }
                return this.collection;
            }
        }
        #endregion

        #region Methods

        private void RefreshConfiguration(Boolean forceRefresh = false)
        {
            this.configuration = null;
            this.configurationSection = null;
            this.collection = null;
        }
        private Boolean SettingExists(string key)
        {
            return this.Collection
                .AllKeys
                .Select(k => k.ToLower())
                .Contains(key.ToLower());
        }

        public void SaveSetting(string key, object value)
        {
            try
            {
                RefreshConfiguration();
                string stringValue = (value == null) ? "" : value.ToString();
                if (!SettingExists(key))
                    this.Collection.Add(new KeyValueConfigurationElement(key, stringValue));
                else
                    this.Collection[key].Value = stringValue;
                this.Configuration.Save();
            }
            catch (Exception exception)
            {
                RefreshConfiguration(true);
                throw HelperMethods.CreateException(exception, "خطا در ذخیره تنظیمات برای {0} . متن خطا : {1}", key, exception.Message);
            }
        }
        public T LoadSetting<T>(string key, T defaultValue, Type propertyType)
        {
            string stringValue;
            try
            {

                lock (this)
                {
                    RefreshConfiguration();
                    if (!SettingExists(key))
                        SaveSetting(key, defaultValue);

                    stringValue = this.Collection[key].Value;
                }
            }
            catch (Exception exception)
            {
                throw HelperMethods.CreateException(exception, "خطا در بازیابی تنظیمات برای {0} . متن خطا : {1}", key, exception.Message);
            }
            try
            {
                return (T)HelperMethods.ConvertFromString(stringValue, propertyType);
            }
            catch
            {
                LogService.LogWarning("مقدار تعیین شده برای {0} برابر با {1} می باشد که به عنوان یک {2} معتبر نیست.", key, stringValue, typeof(T).Name);
                SaveSetting(key, defaultValue);
                return defaultValue;
            }
        }

        #endregion

        #region Event Handlers
        private void OnConfigurationFileChanged()
        {
            if (this.ConfigurationFileChanged != null)
                this.ConfigurationFileChanged();
        }
        public event ConfigurationFileChangedHandler ConfigurationFileChanged;
        public delegate void ConfigurationFileChangedHandler();
        #endregion
    }
}

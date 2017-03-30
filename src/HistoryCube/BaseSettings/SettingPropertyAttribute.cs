using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Settings
{
    public class SettingPropertyAttribute : Attribute
    {
        public object DefaultValue { get; private set; }

        public SettingPropertyAttribute(object defaultValue)
        {
            this.DefaultValue = defaultValue;
        }
    }
}

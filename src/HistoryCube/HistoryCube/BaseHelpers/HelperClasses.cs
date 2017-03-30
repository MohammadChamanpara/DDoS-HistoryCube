using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;

namespace Library
{
    public static class HelperClasses
    {
        #region EnumDescriptionConverter
        public class EnumDescriptionConverter : EnumConverter
        {
            public EnumDescriptionConverter(Type type) : base(type) { }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                FieldInfo fieldInfo = this.EnumType.GetField(Enum.GetName(this.EnumType, value));
                DescriptionAttribute description = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                if (description != null)
                    return description.Description;
                else
                    return base.ConvertTo(context, culture, value, destinationType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                foreach (FieldInfo fi in this.EnumType.GetFields())
                {
                    DescriptionAttribute description = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                    if ((description != null) && ((string)value == description.Description))
                        return Enum.Parse(this.EnumType, fi.Name);
                }
                return base.ConvertFrom(context, culture, value);
            }
        }
        #endregion

        #region LibraryException
        public class LibraryException : Exception
        {
            public LibraryException(string messageFormat,params object[] arguments) : base(messageFormat.FormatWith(arguments)) { }
            public LibraryException(string message) : base(message) { }
            public LibraryException(string message, Exception innerException) : base(message, innerException) { }

        }
        #endregion

        #region PersianDateTime
        public class PersianDateTime
        {
            #region Variables
            private DateTime dateTime = new DateTime();
            #endregion

            #region Constructors
            public PersianDateTime(DateTime dateTime)
            {
                this.dateTime = dateTime;
            }
            public PersianDateTime(string persianDateTime) : this(persianDateTime, '/', ':') { }
            public PersianDateTime(string persianDateTime, char dateSeparator) : this(persianDateTime, dateSeparator, ':') { }
            public PersianDateTime(string persianDateTime, char dateSeparator, char timeSeparator)
            {
                string datePart = persianDateTime.Substring(0, 10);
                var dateParts = datePart.Split(dateSeparator);

                string[] timeParts = new string[0];
                if (persianDateTime.Length > 10)
                {
                    string timePart = persianDateTime.Substring(10);
                    timeParts = timePart.Split(timeSeparator);
                }
                PersianCalendar persianCalendar = new PersianCalendar();
                int persianYear = int.Parse(dateParts[0]);
                int persianMonth = int.Parse(dateParts[1]);
                int persianDay = int.Parse(dateParts[2]);

                int hour = 0;
                if (timeParts.Length > 0)
                    hour = int.Parse(timeParts[0]);

                int minute = 0;
                if (timeParts.Length > 1)
                    minute = int.Parse(timeParts[1]);

                int second = 0;
                if (timeParts.Length > 2)
                    second = int.Parse(timeParts[2]);

                int millisecond = 0;
                if (timeParts.Length > 3)
                    millisecond = int.Parse(timeParts[3]);

                this.dateTime = persianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, millisecond);
            }

            #endregion

            #region Methods
            public DateTime ToDateTime()
            {
                return this.dateTime;
            }
            #endregion

            #region Properties
            public string DateSparator { get; set; }
            public string PersianDateTimeString
            {
                get
                {
                    try
                    {
                        PersianCalendar persianCalender = new PersianCalendar();
                        return string.Format
                        (
                            "{0} {1:00}:{2:00}:{3:00}",
                            this.PersianDateString,
                            persianCalender.GetHour(dateTime),
                            persianCalender.GetMinute(dateTime),
                            persianCalender.GetSecond(dateTime)
                        );
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            public string PersianDateString
            {
                get
                {
                    try
                    {
                        PersianCalendar persianCalender = new PersianCalendar();
                        return string.Format
                        (
                            "{0:0000}{3}{1:00}{3}{2:00}",
                            persianCalender.GetYear(dateTime),
                            persianCalender.GetMonth(dateTime),
                            persianCalender.GetDayOfMonth(dateTime),
                            this.DateSparator
                        );
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            #endregion
        }
        #endregion

        #region SingletonProvider
        public sealed class SingletonProvider<T>
            where T : new()
        {
            private static T instance;
            private static object syncRoot = new Object();

            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (syncRoot)
                        {
                            if (instance == null)
                                instance = new T();
                        }
                    }

                    return instance;
                }
            }
        }
        #endregion

        #region TimeTools
        public class TimeTools
        {
            private DateTime startTime;
            public void Start()
            {
                startTime = DateTime.Now;
            }
            public void Stop()
            {
                this.EllapsedTime += (DateTime.Now - startTime).TotalSeconds;
            }

            public double EllapsedTime { get; set; }
            public string StringEllapsedTime
            {
                get
                {
                    return this.EllapsedTime.ToString("N2");
                }
            }
        }
        #endregion

    }

}

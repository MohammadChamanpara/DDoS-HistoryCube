using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Objects;
using System.Data.EntityClient;
using Library.Log;

namespace Library
{
    public static class ExtensionMethods
    {
        #region Enum
        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes
                (typeof(DescriptionAttribute), false);

            return (attributes.Length > 0)
                ? attributes[0].Description
                : enumValue.ToString();
        }
        #endregion

        #region String
        public static string FormatWith(this string instance, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, instance, args);
        }
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        #endregion

        #region Exception
        public static string CompleteMessages(this Exception exception)
        {
            string message = "";
            while (exception != null)
            {
                if (message.HasValue())
                    message += "\r\n\t-- Inner Exception --\r\n\t";
                message += exception.Message;

                if (exception is ReflectionTypeLoadException)
                {
                    var loadException = (exception as ReflectionTypeLoadException);
                    if (loadException.LoaderExceptions != null && loadException.LoaderExceptions.Count() > 0)
                    {
                        message += "\r\n\t-- Loader Exception --\r\n\t";
                        message += loadException.LoaderExceptions.First().Message;
                    }
                }

                exception = exception.InnerException;
            }
            return message;
        }
        public static string CompleteStackTraces(this Exception exception)
        {
            string stackTrace = "";
            while (exception != null)
            {
                if (stackTrace.HasValue())
                    stackTrace += "\r\n\t-- Inner Exception --\r\n\t";
                stackTrace += exception.StackTrace;
                exception = exception.InnerException;
            }
            return stackTrace;
        }
        public static byte[] Serialize(this Exception exception)
        {
            if (exception == null)
                return null;
            return new ExceptionWrapper(exception).ToByteArray();
        }
        #endregion

        #region DateTime
        public static string ToPersianDateTime(this DateTime dateTime, string separator = "/")
        {
            HelperClasses.PersianDateTime persianDateTime = new HelperClasses.PersianDateTime(dateTime);
            persianDateTime.DateSparator = separator;
            return persianDateTime.PersianDateTimeString;
        }
        public static string ToPersianDate(this DateTime dateTime, string separator = "/")
        {
            HelperClasses.PersianDateTime persianDateTime = new HelperClasses.PersianDateTime(dateTime);
            persianDateTime.DateSparator = separator;
            return persianDateTime.PersianDateString;
        }
        #endregion

        #region Int
        public static string DigitGrouping(this int number)
        {
            string sign = "";
            if (number < 0)
            {
                number = -number;
                sign = "-";
            }
            string stringNumber = number.ToString();

            if (stringNumber.Length > 3)
                stringNumber = stringNumber.Insert(stringNumber.Length - 3, ",");
            if (stringNumber.Length > 7)
                stringNumber = stringNumber.Insert(stringNumber.Length - 7, ",");
            if (stringNumber.Length > 11)
                stringNumber = stringNumber.Insert(stringNumber.Length - 11, ",");
            return sign + stringNumber;
        }
        #endregion

        #region Assembly
        public static string FileName(this Assembly assembly)
        {
            return System.IO.Path.GetFileNameWithoutExtension(assembly.Location);
        }

        public static string Title(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != "")
                {
                    return titleAttribute.Title;
                }
            }
            return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
        }

        public static string Version(this Assembly assembly)
        {
            return assembly.GetName().Version.ToString();
        }
        public static string TypeName(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }
        public static string Description(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }

        public static string Product(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }

        public static string Copyright(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        public static string Company(this Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
        #endregion

        #region IQuariable
        public static Boolean IsEmpty<T>(this IQueryable<T> iQueryable)
        {
            return (iQueryable == null || iQueryable.Count() == 0);
        }
        #endregion
    }
}

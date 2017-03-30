using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using Library.Log;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Diagnostics;

namespace Library
{
    public static class HelperMethods
    {
        public static List<T> LoadPlugins<T>()
        {
            return LoadPlugins<T>(AppDomain.CurrentDomain.BaseDirectory + "\\Plugins");
        }
        public static List<T> LoadPlugins<T>(string directory)
        {
            try
            {
                List<T> plugins = new List<T>();

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (!Directory.Exists(directory))
                {
                    throw HelperMethods.CreateException("مسیر ذخیره پلاگین ها {0} موجود نیست.", directory);
                }
                Type interfaceType = typeof(T);
                foreach (var file in Directory.GetFiles(directory, "*.dll"))
                {
                    foreach (var type in Assembly.LoadFile(file).GetTypes())
                    {
                        if
                        (
                            interfaceType.IsAssignableFrom(type) &&
                            interfaceType != type &&
                            type.ContainsGenericParameters == false
                        )
                        {
                            T instance = (T)Activator.CreateInstance(type);
                            plugins.Add(instance);
                        }
                    }
                }
                return plugins;
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                if (exception is ReflectionTypeLoadException)
                {
                    var loadException = (exception as ReflectionTypeLoadException);
                    if (loadException.LoaderExceptions != null && loadException.LoaderExceptions.Count() > 0)
                        message = loadException.LoaderExceptions.First().Message;
                }
                throw HelperMethods.CreateException("خطا هنگام لود کردن پلاگین ها. متن خطا : " + message);
            }
        }

        public static HelperClasses.LibraryException CreateException(string messageFormat, params object[] parameters)
        {
            return CreateException(null, messageFormat, parameters);
        }
        public static HelperClasses.LibraryException CreateException(Exception innerException, string messageFormat, params object[] parameters)
        {
            return new HelperClasses.LibraryException(string.Format(messageFormat, parameters), innerException);
        }

        public static void CallWithTimeout(Action action, int timeoutSeconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutSeconds * 1000))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        public static void RepairXml(string fileName)
        {
            if (File.Exists(fileName))
            {
                var bytes = File.ReadAllBytes(fileName);
                if (bytes.Length > 2 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                    File.WriteAllBytes(fileName, bytes.Skip(3).ToArray());
            }
        }

        public static T ConvertFromString<T>(string value)
        {
            return (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value));
        }
        public static object ConvertFromString(string value, Type destinationType)
        {
            return TypeDescriptor.GetConverter(destinationType).ConvertFrom(value);
        }

        public static string GetFullPath(string fileAddress)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileAddress);
        }
    }
}

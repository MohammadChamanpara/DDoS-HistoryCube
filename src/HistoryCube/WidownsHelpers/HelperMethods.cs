using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using Library.Windows.Forms;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using System.ComponentModel;

namespace Library.Windows.WindowsForms.Helpers
{
    public static class WindowsHelperMethods
    {

        public static string ShowPasswordDialog(string caption)
        {
            var form = new PasswordDialogForm();
            form.Text = caption;
            form.ShowDialog();
            return form.passwordTextBox.Text;
        }

        public static DialogResult ShowQuestionBox(string messageFormat, params object[] messageArguments)
        {
            return new MessageForm("تأیید", messageFormat, messageArguments, HistoryCube.Properties.Resources.Question, MessageForm.FormMode.Question).ShowDialog();
        }
        public static void ShowExceptionMessage(Exception exception,string messageFormat, params object[] messageArguments)
        {
            string message="{0}\r\n\r\nمتن خطا : {1}\r\n\r\nمحل رویداد خطا : {2} ".
                FormatWith
                (
                    messageFormat.FormatWith(messageArguments),
                    exception.CompleteMessages(),
                    exception.CompleteStackTraces()
                );
            new MessageForm("خطا", message,new object[0], HistoryCube.Properties.Resources.Error, MessageForm.FormMode.Message).ShowDialog();
        }
        public static void ShowErrorMessage(string messageFormat, params object[] messageArguments)
        {
            new MessageForm("خطا", messageFormat, messageArguments, HistoryCube.Properties.Resources.Error, MessageForm.FormMode.Message).ShowDialog();
        }
        public static void ShowWarningMessage(string messageFormat, params object[] messageArguments)
        {
            new MessageForm("هشدار", messageFormat, messageArguments, HistoryCube.Properties.Resources.Warning, MessageForm.FormMode.Message).ShowDialog();
        }
        public static void ShowInformationMessage(string messageFormat, params object[] messageArguments)
        {
            new MessageForm("اطلاعات", messageFormat, messageArguments, HistoryCube.Properties.Resources.Information, MessageForm.FormMode.Message).ShowDialog();
        }
        public static T ConvertFromString<T>(string value)
        {
            return (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value));
        }
        public static void ClearSearchControls(Control parentControl)
        {
            if (parentControl is CheckBox)
            {
                var checkBox = (parentControl as CheckBox);
                if (checkBox.ThreeState == true)
                    checkBox.CheckState = CheckState.Indeterminate;
                else
                    checkBox.Checked = false;
            }
            else if (parentControl is TextBox)
                (parentControl as TextBox).Clear();
            else if (parentControl is MaskedTextBox)
                (parentControl as MaskedTextBox).Clear();
            else if (parentControl is ComboBox)
                (parentControl as ComboBox).SelectedIndex = -1;
            else
                foreach (Control childControl in parentControl.Controls)
                    ClearSearchControls(childControl);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library.Windows.WindowsForms.Helpers;
using Library;

namespace HistoryCube
{
    public partial class MainForm : Form
    {
        #region Constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Events

        private void buttonGeneratePath_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxGeneratePath.Text = dialog.FileName;
                ApplicationSettings.Instance.GenerateFilePath = dialog.FileName;
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationSettings.Instance.GenerateFilePath = textBoxGeneratePath.Text;
                int tuplesCount = StoneHandler.GenerateDataSet(textBoxGeneratePath.Text);
                WindowsHelperMethods.ShowInformationMessage("فایل DataSet با {0} Tuple موفقیت آمیز ایجاد گردید.", tuplesCount);
            }
            catch (Exception exception)
            {
                HandleUIException(exception, "بروز خطا هنگام ایجاد فایل DataSet.");
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ApplicationSettings.Instance.Load();

            textBoxGeneratePath.Text = ApplicationSettings.Instance.GenerateFilePath;
            textBoxLoadPath.Text = ApplicationSettings.Instance.LoadFilePath;
            textBoxConvertFromPath.Text = ApplicationSettings.Instance.ConvertFromPath;
            textBoxConvertToPath.Text = ApplicationSettings.Instance.ConvertToPath;
            textBoxConvertReportPath.Text = ApplicationSettings.Instance.ConvertReportPath;

            propertyGrid1.SelectedObject = ApplicationSettings.Instance;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ApplicationSettings.Instance.Save();
            }
            catch (Exception exception)
            {
                HandleUIException(exception, "بروز خطا هنگام ثبت تنظیمات.");
            }
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationSettings.Instance.LoadFilePath = textBoxLoadPath.Text;
                int tuplesCount = StoneHandler.LoadDataSet(textBoxLoadPath.Text);
                WindowsHelperMethods.ShowInformationMessage("فایل DataSet با {0} Tuple موفقیت آمیز خوانده شد.", tuplesCount);
            }
            catch (Exception exception)
            {
                HandleUIException(exception, "Error when loading dataset file.");
            }
        }
        private void buttonLoadFilePath_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxLoadPath.Text = dialog.FileName;
                ApplicationSettings.Instance.LoadFilePath = dialog.FileName;
            }
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {
            //BinaryMemory bm = new BinaryMemory();
            //var s = DateTime.Now;
            //bm.Add(1);
            //bm.Add(22);
            //bm.Add(333);
            //bm.Store(4444);
            //bm.Store(55555);
            //bm.Store(666666);
            //bm.Store(7777777);
            //bm.Store(88888888);
            //var st = (DateTime.Now - s).TotalMilliseconds;
            //MessageBox.Show("Stored in " + st);

            //s = DateTime.Now;
            //var r = bm.RetrieveItems();
            //st = (DateTime.Now - s).TotalMilliseconds;
            //MessageBox.Show("Retrieved in " + st);

            //MessageBox.Show(bm.Contains(3333).ToString());

            //r.ForEach(x => MessageBox.Show(x.ToString()));
            //return;
            try
            {
                HelperClasses.TimeTools timer = new HelperClasses.TimeTools();
                timer.Start();
                StoneHandler.Run();
                timer.Stop();
                WindowsHelperMethods.ShowInformationMessage("عملیات با موفقیت خاتمه یافت. زمان اجرا : {0} ثانیه.", timer.StringEllapsedTime);

            }
            catch (Exception exception)
            {
                HandleUIException(exception, "بروز خطا هنگام اجرای Stone.");
            }
        }
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                ApplicationSettings.Instance.Save();
            }
            catch (Exception exception)
            {
                HandleUIException(exception, "بروز خطا هنگام ذخیره تنظیمات.");
            }
        }
        private void convertFromPathButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxConvertFromPath.Text = dialog.FileName;
                ApplicationSettings.Instance.ConvertFromPath = dialog.FileName;
            }
        }
        private void buttonConvertToPath_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxConvertToPath.Text = dialog.FileName;
                ApplicationSettings.Instance.ConvertToPath = dialog.FileName;
            }
        }
        private void buttonConvertReportPath_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxConvertReportPath.Text = dialog.FileName;
                ApplicationSettings.Instance.ConvertReportPath = dialog.FileName;
            }
        }
        private void buttonConvert_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationSettings.Instance.ConvertFromPath = textBoxConvertFromPath.Text;
                ApplicationSettings.Instance.ConvertToPath = textBoxConvertToPath.Text;
                ApplicationSettings.Instance.ConvertReportPath = textBoxConvertReportPath.Text;
                HelperClasses.TimeTools timer = new HelperClasses.TimeTools();
                timer.Start();
                var inputPath = textBoxConvertFromPath.Text;
                var outputPath = textBoxConvertToPath.Text;
                var reportPath = textBoxConvertReportPath.Text;
                DataSetConverter.Convert(inputPath, outputPath, reportPath);
                timer.Stop();
                WindowsHelperMethods.ShowInformationMessage("عملیات با موفقیت خاتمه یافت. زمان اجرا : {0} ثانیه.", timer.StringEllapsedTime);
            }
            catch (Exception exception)
            {
                HandleUIException(exception, "بروز خطا هنگام تبدیل فایل ورودی.");
            }
        }
        #endregion

        #region Methods
        private static void HandleUIException(Exception exception, string message)
        {
            WindowsHelperMethods.ShowExceptionMessage(exception, message);
        }

        #endregion
    }
}

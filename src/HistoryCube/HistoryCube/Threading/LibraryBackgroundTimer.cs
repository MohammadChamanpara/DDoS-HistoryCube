using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using Library.Log;

namespace Library.Threading
{
    public class LibraryBackgroundTimer : BackgroundWorker
    {
        #region Variables
        private ManualResetEvent intervalManualReset;
        private enum ProcessStatus { Created, Running, JobCompleted, ExceptionOccured };
        private ProcessStatus processStatus = new ProcessStatus();
        private DateTime startTime = new DateTime();
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interval in milliseconds at which 
        /// to raise the Library.Threading.LibraryBackgroundTimer.Dowork event.
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets the AliveTime in milliseconds. After this Time LibraryBackgroundTimer will be stopped.
        /// </summary>
        public int AliveTime { get; set; }

        public string PersianDescription { get; set; }

        #endregion

        #region Constructor
        public LibraryBackgroundTimer()
        {
            this.AliveTime = 0;
            this.processStatus = ProcessStatus.Created;
            this.WorkerSupportsCancellation = true;
            this.Interval = 1000;
        }
        #endregion

        #region Methods
        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);
            if (processStatus == ProcessStatus.ExceptionOccured)
                LogService.LogWarning("اجرای {0} خاتمه یافت.", this.PersianDescription);
            processStatus = ProcessStatus.JobCompleted;
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            while (!this.CancellationPending)
            {
                try
                {
                    if (this.AliveTime > 0)
                        if ((DateTime.Now - this.startTime).TotalMilliseconds >= this.AliveTime)
                            this.Stop();

                    base.OnDoWork(e);
                    this.Sleep();
                }
                catch (Exception exception)
                {
                    LogService.LogException(exception, "بروز خطا هنگام انجام عملیات در {0}.", this.PersianDescription);
                    this.processStatus = ProcessStatus.ExceptionOccured;
                    this.Stop();
                }
            }
            if (e != null)
                e.Cancel = true;
        }

        public void Start()
        {
            this.startTime = DateTime.Now;
            this.processStatus = ProcessStatus.Running;
            if (this.IsBusy)
                return;

            this.intervalManualReset = new ManualResetEvent(false);
            this.RunWorkerAsync();
        }
        public void Stop()
        {
            this.CancelAsync();
            this.WakeUp();
            this.Dispose(true);
        }
        public void WakeUp()
        {
            if (this.intervalManualReset != null)
                this.intervalManualReset.Set();
        }
        private void Sleep()
        {
            if (this.intervalManualReset != null)
            {
                this.intervalManualReset.Reset();
                this.intervalManualReset.WaitOne(this.Interval);
            }
        }
        public void Activate()
        {
            if (!this.IsBusy)
                LogService.LogSuccess("اجرای {0} توسط پروسه فعال ساز فعال شد.", this.PersianDescription);
            this.Start();
        }
        #endregion
    }
}

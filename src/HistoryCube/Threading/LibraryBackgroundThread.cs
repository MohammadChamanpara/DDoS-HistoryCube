using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using Library.Log;

namespace Library.Threading
{
    public class LibraryBackgroundThread : BackgroundWorker
    {
        #region Properties
        public string PersianDescription { get; set; }
        #endregion

        #region Constructor
        public LibraryBackgroundThread()
        {
            this.WorkerSupportsCancellation = true;
        }
        #endregion

        #region Methods
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {
                base.OnDoWork(e);
            }
            catch (Exception exception)
            {
                LogService.LogException(exception, "بروز خطا هنگام انجام عملیات در {0}.", this.PersianDescription);
            }
        }
        public void Run()
        {
            if (this.IsBusy)
                return;
            this.RunWorkerAsync();
        }
        public void Stop()
        {
            this.CancelAsync();
            this.Dispose(true);
        }
        #endregion
    }
}

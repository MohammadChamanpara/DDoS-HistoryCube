using Library.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public static class PersistThread
    {
        static LibraryBackgroundTimer timer = new LibraryBackgroundTimer();
        public static void Start()
        {
            timer.Interval = ApplicationSettings.Instance.PersistThreadInterval * 1000;
            timer.DoWork += Timer_DoWork;
            timer.Start();
        }

        private static void Timer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            StoneHandler.Persist();
        }
    }
}

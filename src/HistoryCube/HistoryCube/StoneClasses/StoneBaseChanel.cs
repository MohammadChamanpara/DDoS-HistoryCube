using Library;
using Library.Windows.WindowsForms.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HistoryCube
{
    public abstract class StoneBaseChanel
    {
        public abstract double Capacity { get; }
        public void Add(DataSetTuple dataSetTuple)
        {
            ForwardToServer(dataSetTuple);
        }

        public static void ForwardToServer(DataSetTuple dataSetTuple)
        {
            File.AppendAllText(ApplicationSettings.Instance.ServerInputLinkFile, dataSetTuple.ToString() + "\r\n");
        }
        public Boolean ProbabilisticAdd(DataSetTuple dataSetTuple, double probability)
        {
            int seed = new Random().Next(100);
            var value = new Random(seed).NextDouble();
            if (value < probability)
            {
                this.Add(dataSetTuple);
                return true;
            }
            return false;
        }

    }
}

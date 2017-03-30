using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;

namespace HistoryCube
{
    public static class DataAggregator
    {
        public static StoneWindow Window { get; set; }

        public static void AddTuple(DataSetTuple dataSetTuple)
        {
            Window.AddTuple(dataSetTuple);
        }
        public static void Initialize()
        {
            Window = new StoneWindow
            (
                ApplicationSettings.Instance.WindowSize,
                ApplicationSettings.Instance.WindowAdvance
            );
        }
    }
}

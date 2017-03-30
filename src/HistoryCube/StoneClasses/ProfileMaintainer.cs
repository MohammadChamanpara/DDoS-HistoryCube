using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;
using Library.Windows.WindowsForms.Helpers;
using HistoryCube.Model;
namespace HistoryCube
{
    public static class ProfileMaintainer
    {
        #region Properties
        public static StoneDay[] Days;
        public static List<StoneCluster> AcquaintanceList { get; set; }
        #endregion

        #region Methods
        public static void ComputeDayWeights()
        {
            var wH = ApplicationSettings.Instance.HistoricWeight;
            if (wH < 0)
                throw new HelperClasses.LibraryException("The value of historic weight is {0} but it must be a positive value.", wH.ToString());
            var D = ApplicationSettings.Instance.HistoryDaysCount;
            Days = new StoneDay[D];

            for (int d = 0; d < D; d++)
                Days[d] = new StoneDay(d);

            if (wH == 1)
                for (int d = 0; d < D; d++)
                    Days[d].Weight = 1 / wH;
            else
            {
                Days[D - 1].Weight = (1 - wH) / (1 - Math.Pow(wH, D));

                for (int d = D - 2; d >= 0; d--)
                    Days[d].Weight = Math.Pow(wH, D - 1 - d) * Days[D - 1].Weight;
            }
        }

        public static void Refresh()
        {
            AcquaintanceList = new List<StoneCluster>();
            foreach (var dbCluster in DB.NewContext.ClusterHistories.Where(x => x.Day < ApplicationSettings.Instance.HistoryDaysCount))
            {
                var cluster = new StoneCluster(dbCluster.ClusterPrefix, dbCluster.W, dbCluster.T, dbCluster.F);
                AcquaintanceList.Add(cluster);
            }
        }

        #endregion
    }
}

using HistoryCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public static class OfflineDB
    {
        public static BinaryMemory GroupRatioHistories = new BinaryMemory();
        public static BinaryMemory GroupRateHistories = new BinaryMemory();
        public static BinaryMemory GroupClusterHistories = new BinaryMemory();

        public static List<ClusterHistory> ClusterHistories = new List<ClusterHistory>();

        public static void RefreshFromDB()
        {
            foreach
            (
                var groupRateHistory in DB.NewContext.GroupRateHistories
                .Where(x => x.Day < ApplicationSettings.Instance.HistoryDaysCount)
                .ToList()
            )
            {
                GroupRateHistories.Add(groupRateHistory.ToBinaryValue());
            }

            foreach
            (
                var groupRatioHistory in DB.NewContext.GroupRatioHistories
                .Where(x => x.Day < ApplicationSettings.Instance.HistoryDaysCount)
                .ToList()
            )
            {
                GroupRatioHistories.Add(groupRatioHistory.ToBinaryValue());
            }

            foreach
            (
                var groupClusterHistory in DB.NewContext.GroupClusterHistories
                .Where(x => x.Day < ApplicationSettings.Instance.HistoryDaysCount)
                .ToList()
            )
            {
                GroupClusterHistories.Add(groupClusterHistory.ToBinaryValue());
            }

            foreach
            (
                var clusterHistory in DB.NewContext.ClusterHistories
                .Where(x => x.Day < ApplicationSettings.Instance.HistoryDaysCount)
                .ToList()
            )
            {
                ClusterHistories.Add(clusterHistory);
            }
        }
    }
}

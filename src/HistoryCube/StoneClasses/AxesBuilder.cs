using HistoryCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class AxesBuilder
    {
        public static List<StoneCluster> Clustres = new List<StoneCluster>();

        public static StoneFeatures Origin = new StoneFeatures();

        public static int GroupFunction(StoneFeatures features)
        {
            int x, y, z;

            x = (features.LifeTime > Origin.LifeTime) ? 1 : 0;
            y = (features.Weight > Origin.Weight) ? 1 : 0;
            z = (features.Frequency > Origin.Frequency) ? 1 : 0;

            return x * 4 + y * 2 + z;
        }
        public static void Add(StoneCluster stoneCluster)
        {
            if (!Clustres.Contains(stoneCluster))
                Clustres.Add(stoneCluster);
        }
        public static void Persist(Stone_DBEntities db)
        {
            foreach (var clusterHistory in db.ClusterHistories.Where(x => x.Day <= ApplicationSettings.Instance.HistoryDaysCount))
                clusterHistory.Day = clusterHistory.Day + 1;

            foreach (StoneCluster cluster in AxesBuilder.Clustres)
            {
                var newClusterHistory = new ClusterHistory()
                {
                    ClusterPrefix = cluster.Prefix,
                    Day = 0,
                    W = cluster.Weight,
                    T = cluster.LifeTime,
                    F = cluster.Frequency
                };
                db.ClusterHistories.AddObject(newClusterHistory);
            }
        }

        public static void UpdateOrigin()
        {
            int D = ApplicationSettings.Instance.HistoryDaysCount;

            var clusters = DB.NewContext.ClusterHistories
                .Select(x => x.ClusterPrefix)
                .Distinct()
                .ToList();

            double[] wSourceCluster = new double[clusters.Count];
            double[] weightedF = new double[clusters.Count];
            double[] weightedW = new double[clusters.Count];
            double[] weightedT = new double[clusters.Count];

            for (int i = 0; i < clusters.Count; i++)
            {
                var cluster = clusters[i];
                var thisClusterHistories = DB.NewContext.ClusterHistories.Where(x => x.ClusterPrefix == cluster);

                for (int day = 0; day < D; day++)
                {
                    var dayHistory = thisClusterHistories.FirstOrDefault(x => x.Day == day);

                    if (dayHistory == null)
                        continue;

                    wSourceCluster[i] += ProfileMaintainer.Days[day].Weight;
                    weightedF[i] += ProfileMaintainer.Days[day].Weight * dayHistory.F;
                    weightedT[i] += ProfileMaintainer.Days[day].Weight * dayHistory.T;
                    weightedW[i] += ProfileMaintainer.Days[day].Weight * dayHistory.W;
                }
                weightedF[i] = 1 / wSourceCluster[i] * weightedF[i];
                weightedT[i] = 1 / wSourceCluster[i] * weightedT[i];
                weightedW[i] = 1 / wSourceCluster[i] * weightedW[i];
            }
            int p = (int)Math.Truncate(0.95 * wSourceCluster.Sum());

            weightedF.ToList().Sort();

            Origin.Frequency = (int)weightedF[p];
            Origin.Weight = (int)weightedW[p];
            Origin.LifeTime = (int)weightedT[p];
        }
    }
}
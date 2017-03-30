using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HistoryCube.Model;
using Library;

namespace HistoryCube
{
    public static class DCC
    {
        #region Properties
        public static StoneGroup[] Groups = new StoneGroup[8];
        #endregion

        #region Methods
        public static void Receive(DataAggregatorTuple daTuple)
        {
            int newGroupIndex = AxesBuilder.GroupFunction(daTuple.NewFeatures);
            int oldGroupIndex = AxesBuilder.GroupFunction(daTuple.OldFeatures);
            if (newGroupIndex != oldGroupIndex)
            {
                if (daTuple.NewFeatures.HasValue)
                {
                    if (newGroupIndex != 0)
                        Groups[newGroupIndex].AddCluster(daTuple.SourceCluster);
                }
                if (daTuple.OldFeatures.HasValue && Groups[oldGroupIndex].Clusters.Count > 0)
                {
                    if (oldGroupIndex != 0)
                        Groups[oldGroupIndex].RemoveCluster(daTuple.SourceCluster);
                }
            }

            int sumRatio = Groups.Sum(x => x.Clusters.Count);

            foreach (var group in Groups)
                group.Rc = (double)group.Clusters.Count / sumRatio;


            for (int i = 0; i < 8; i++)
                if (Math.Abs(Groups[i].Rc - Groups[i].Re) > ApplicationSettings.Instance.AlarmTolerance)
                    Alarm(i, Groups[i].Rc, Groups[i].Re, ApplicationSettings.Instance.AlarmTolerance);

            AxesBuilder.Add(daTuple.SourceCluster);
        }
        private static void Alarm(int group, double rc, double re, int tolerance)
        {
            throw new AlarmException(group, rc, re, tolerance);
        }
        public static void Persist(Stone_DBEntities db)
        {
            foreach (StoneGroup group in Groups)
                group.Persist(db);
        }
        public static void Refresh()
        {
            foreach (var group in Groups)
                group.Refresh();
        }
        #endregion
    }
}

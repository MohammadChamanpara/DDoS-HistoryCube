using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public static class FilterComponent
    {
        #region constructor
        static FilterComponent()
        {
            if (!IsActive)
                return;

            GroupChanels = new List<StoneGroupChanel>();
            foreach (var group in DCC.Groups)
                GroupChanels.Add(new StoneGroupChanel(group));

            SuspiciousChanel = new StoneSuspiciousChanel();
        }

        #endregion

        #region Properties
        public static Boolean IsActive
        {
            get
            {
                return ApplicationSettings.Instance.FilterComponentIsActive;
            }
            set
            {
                ApplicationSettings.Instance.FilterComponentIsActive = value;
            }
        }
        public static List<StoneGroupChanel> GroupChanels { get; set; }
        public static StoneSuspiciousChanel SuspiciousChanel { get; set; }
        #endregion

        #region Methods
        public static void AddTuple(DataSetTuple dataSetTuple)
        {
            if (!IsActive)
            {
                StoneBaseChanel.ForwardToServer(dataSetTuple);
                return;
            }
            
            int groupNumber = AxesBuilder.GroupFunction(dataSetTuple.StoneFeatures);
            if (groupNumber == 0)
                GroupChanels[0].Add(dataSetTuple);
            else
            {
                var clusters = ProfileMaintainer.AcquaintanceList.Where(x => x.Prefix == dataSetTuple.ClusterPrefix);
                if (clusters.Count() > 0)
                {
                    Boolean added = false;
                    var cluster = clusters.First();
                    foreach (var group in DCC.Groups)
                    {
                        var probability = cluster.ProbabilityInGroup(group);
                        var chanel = GroupChanels[group.Number];
                        added = chanel.ProbabilisticAdd(dataSetTuple, probability * chanel.Capacity);
                        if (added)
                            break;
                    }
                    if (!added)
                        SuspiciousChanel.Add(dataSetTuple);
                }
                else
                    SuspiciousChanel.Add(dataSetTuple);
            }

        }

        #endregion
    }
}


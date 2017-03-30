using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube.Model
{
    public partial class GroupClusterHistory
    {
        public GroupClusterHistory() : base() { }

        public GroupClusterHistory(int day, int groupNumber, int clusterPrefix)
        {
            this.Day = day;
            this.GroupNumber = groupNumber;
            this.ClusterPrefix = clusterPrefix;
        }
        public int ToBinaryValue()
        {
            return BinaryMemory.ConcatNumbers(GroupNumber, Day, ClusterPrefix);
        }
        public void FromBinaryValue(int binaryValue)
        {
            int groupNumber;
            int day;
            int clusterPrefix;
            BinaryMemory.SplitNumbers(binaryValue, out groupNumber, out day, out clusterPrefix);
            this.GroupNumber = groupNumber;
            this.Day = day;
            this.ClusterPrefix = clusterPrefix;
        }
    }
}

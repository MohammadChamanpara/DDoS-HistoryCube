using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class DataAggregatorTuple
    {
        public StoneCluster SourceCluster { get; set; }

        public StoneFeatures OldFeatures { get; set; }

        public StoneFeatures NewFeatures { get; set; }

        public double StartTime { get; set; }
        public DataAggregatorTuple()
        {
            this.OldFeatures = new StoneFeatures();
            this.NewFeatures = new StoneFeatures();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;

namespace HistoryCube
{
    public class StoneWindow
    {
        private int Size { get; set; }
        private int Advance { get; set; }
        private double T0 { get; set; }
        private Boolean Initialized { get; set; }
        public List<StoneCluster> Clusters { get; set; }

        public StoneWindow(int size, int advance)
        {
            this.Clusters = new List<StoneCluster>();
            this.Size = size;
            this.Advance = advance;
            this.Initialized = false;
        }
        public void AddTuple(DataSetTuple dataSetTuple)
        {
            if (!this.Initialized)
            {
                StoneHandler.Refresh();
                this.T0 = dataSetTuple.StartTime;
                this.Initialized = true;
            }
            else if ((dataSetTuple.StartTime - this.T0)> this.Size)
                this.Shift(dataSetTuple);
                        
            StoneCluster cluster;
            var findedClusters = this.Clusters.Where(x => x.Prefix == dataSetTuple.ClusterPrefix);
            if (findedClusters.Count() == 0)
            {
                cluster = new StoneCluster(dataSetTuple.ClusterPrefix);
                this.Clusters.Add(cluster);
            }
            else if (findedClusters.Count() == 1)
                cluster = findedClusters.First();
            else
                throw new HelperClasses.LibraryException("{0} کلاستر با پیشوند {1} وجود دارد.", findedClusters.Count(), dataSetTuple.ClusterPrefix);

            cluster.AddTuple(dataSetTuple);
        }

        private void Shift(DataSetTuple dataSetTuple)
        {
            this.T0+=this.Advance;
            foreach (StoneCluster cluster in this.Clusters)
                cluster.Purge(this.T0);
        }
    }
}

using HistoryCube.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class StoneCluster
    {
        #region Properties
        public int Prefix { get; private set; }
        public List<DataSetTuple> DataSetTuples { get; set; }

        private int _frequency = 0;
        public int Frequency
        {
            get
            {
                if (_frequency == 0)
                {
                    if (this.DataSetTuples.Count == 0)
                        _frequency = 0;
                    else
                        _frequency = (int)this.DataSetTuples.Average(x => x.StoneFeatures.Frequency);
                }
                return _frequency;
            }
        }

        private int _weight = 0;
        public int Weight
        {
            get
            {
                if (_weight == 0)
                {
                    if (this.DataSetTuples.Count == 0)
                        _weight = 0;
                    else
                        _weight = (int)this.DataSetTuples.Average(x => x.StoneFeatures.Weight);
                }
                return _weight;
            }
        }

        private int _lifeTime = 0;
        public int LifeTime
        {
            get
            {
                if (_lifeTime == 0)
                {
                    if (this.DataSetTuples.Count == 0)
                        _lifeTime = 0;
                    else
                        _lifeTime = (int)this.DataSetTuples.Average(x => x.StoneFeatures.LifeTime);
                }
                return _lifeTime;
            }
        }

        public int StartTime
        {
            get
            {
                if (this.DataSetTuples.Count == 0)
                    return 0;
                return (int)this.DataSetTuples.First().StartTime;
            }
        }

        public int Rate
        {
            get
            {
                if (this.LifeTime == 0)
                    return 1;
                return this.Weight / this.LifeTime;
            }
        }
        #endregion

        #region Constructor
        public StoneCluster(int prefix, int weight = 0, int lifeTime = 0, int frequency = 0)
        {
            this.Prefix = prefix;
            _weight = weight;
            _frequency = frequency;
            _lifeTime = lifeTime;

            this.DataSetTuples = new List<DataSetTuple>();
        }
        #endregion

        #region Methods
        public void AddTuple(DataSetTuple dataSetTuple)
        {
            _lifeTime = 0;
            _weight = 0;
            _frequency = 0;

            DataAggregatorTuple tuple = new DataAggregatorTuple();

            tuple.OldFeatures.Frequency = this.Frequency;
            tuple.OldFeatures.Weight = this.Weight;
            tuple.OldFeatures.LifeTime = this.LifeTime;

            this.DataSetTuples.Add(dataSetTuple);

            tuple.NewFeatures.Frequency = this.Frequency;
            tuple.NewFeatures.Weight = this.Weight;
            tuple.NewFeatures.LifeTime = this.LifeTime;

            tuple.SourceCluster = this;
            tuple.StartTime = this.StartTime;
            DCC.Receive(tuple);
        }
        public void Purge(double newT0)
        {
            _lifeTime = 0;
            _weight = 0;
            _frequency = 0;

            DataAggregatorTuple tuple = new DataAggregatorTuple();

            tuple.OldFeatures.Frequency = this.Frequency;
            tuple.OldFeatures.Weight = this.Weight;
            tuple.OldFeatures.LifeTime = this.LifeTime;

            this.DataSetTuples.RemoveAll(x => x.StartTime < newT0);

            tuple.NewFeatures.Frequency = this.Frequency;
            tuple.NewFeatures.Weight = this.Weight;
            tuple.NewFeatures.LifeTime = this.LifeTime;

            tuple.SourceCluster = this;
            tuple.StartTime = this.StartTime;

            DCC.Receive(tuple);
        }
        public double ProbabilityInGroup(StoneGroup group)
        {
            double sum = 0;
            foreach (var day in ProfileMaintainer.Days)
                if (this.
                    IsGroupMember(group, day))
                    sum += day.Weight;
            return sum;
        }
        public bool IsGroupMember(StoneGroup group, StoneDay day)
        {
            return
            (
                OfflineDB.GroupClusterHistories.Contains
                (
                    new GroupClusterHistory(day.Number,group.Number, this.Prefix).ToBinaryValue()
                )
            );
        }
        #endregion
    }
}

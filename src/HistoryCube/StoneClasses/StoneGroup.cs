using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HistoryCube.Model;
using Library;

namespace HistoryCube
{
    public class StoneGroup
    {
        #region Properties
        public int Number { get; set; }
        public double Re { get; set; }
        public double Rc { get; set; }
        public List<StoneCluster> Clusters { get; set; }
        public double Weight { get; set; }

        #endregion

        #region Constructor
        public StoneGroup(int number)
        {
            this.Clusters = new List<StoneCluster>();
            this.Number = number;
        }
        #endregion

        #region Methods
        public void AddCluster(StoneCluster stoneCluster)
        {
            if (this.Clusters.Contains(stoneCluster))
                return;
            this.Clusters.Add(stoneCluster);
        }
        public void RemoveCluster(StoneCluster stoneCluster)
        {
            if (!this.Clusters.Contains(stoneCluster))
                return;
            this.Clusters.Remove(stoneCluster);
        }
        public void Persist(Stone_DBEntities db)
        {
            PersistRatios(db);
            PersistClusters(db);
            PersistRates(db);
        }
        private void PersistRatios(Stone_DBEntities db)
        {
            foreach
            (
                var ratioHistory in db.GroupRatioHistories
                .Where
                (
                    x =>
                        x.Day <= ApplicationSettings.Instance.HistoryDaysCount &&
                        x.GroupNumber == this.Number
                )
            )
                ratioHistory.Day = ratioHistory.Day + 1;

            db.GroupRatioHistories.AddObject
             (
                 new GroupRatioHistory()
                 {
                     Day = 0,
                     GroupNumber = this.Number,
                     Ratio = this.Clusters.Count
                 }
             );
        }
        private void PersistClusters(Stone_DBEntities db)
        {
            foreach
            (
                var groupClusterHistory in db.GroupClusterHistories
                .Where
                (
                    x =>
                        x.Day <= ApplicationSettings.Instance.HistoryDaysCount &&
                        x.GroupNumber == this.Number
                )
            )
                groupClusterHistory.Day = groupClusterHistory.Day + 1;

            foreach (StoneCluster cluster in this.Clusters)
            {
                db.GroupClusterHistories.AddObject
                (
                    new GroupClusterHistory()
                    {
                        Day = 0,
                        GroupNumber = this.Number,
                        ClusterPrefix = cluster.Prefix
                    }
                );
            }
        }


        private void PersistRates(Stone_DBEntities db)
        {
            foreach
            (
                var rateHistory in db.GroupRateHistories
                .Where
                (
                    x =>
                        x.Day <= ApplicationSettings.Instance.HistoryDaysCount &&
                        x.GroupNumber == this.Number
                )
            )
                rateHistory.Day = rateHistory.Day + 1;

            db.GroupRateHistories.AddObject
            (
                new GroupRateHistory()
                {
                    Day = 0,
                    GroupNumber = this.Number,
                    Rate = this.Rate()
                }
            );
        }
        int _rate = 0;
        public int Rate(int day)
        {
            if (_rate == 0)
            {
                var list = DB.NewContext
                .GroupRateHistories
                .Where(x => x.Day == day && x.GroupNumber == this.Number);

                if (list.Count()==0)
                    return 1;

                _rate = list.First().Rate;
            }
            return _rate;
        }
        public int Rate()
        {
            return this.Clusters.Sum(x => x.Rate);
        }
        public void Refresh()
        {
            RefreshRatioHistory();
            RefreshRateHistory();
        }
        private void RefreshRatioHistory()
        {
            double sumS = 0;
            double sumM = 0;
            foreach (var day in ProfileMaintainer.Days)
            {
                var dayHistory = DB.NewContext.GroupRatioHistories.Where(x => x.Day == day.Number);
                if (dayHistory.Count()==0)
                {
                    //throw HelperMethods.CreateException("GroupRatioHistoriy does not exist in the day {0}.", day.ToString());
                    this.Re = 1;
                    return;
                }

                var groupHistory = dayHistory.Where(x => x.GroupNumber == this.Number);

                if (groupHistory.Count()==0)
                {
                    //throw HelperMethods.CreateException("GroupRatioHistoriy History does not exist in the day {0} for the group {1}.", day.Number, this.Number);
                    this.Re = 1;
                    return;
                }

                sumS += groupHistory.First().Ratio * day.Weight;
                sumM += dayHistory.Sum(x => x.Ratio) * day.Weight;
            }
            this.Re = sumS / sumM;
        }
        private void RefreshRateHistory()
        {
            double Sums = 0;
            double Summ = 0;
            foreach (var day in ProfileMaintainer.Days)
            {
                Sums += day.Weight * this.Rate(day.Number);
                Summ += day.Weight * DCC.Groups.Sum(x => x.Rate(day.Number));
            }
            this.Weight = Sums / Summ;
        }
        #endregion
    }
}

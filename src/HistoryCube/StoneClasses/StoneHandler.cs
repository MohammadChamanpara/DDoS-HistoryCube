using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;
using Library.Windows.WindowsForms.Helpers;

namespace HistoryCube
{
    public static class StoneHandler
    {
        #region Constructor
        static StoneHandler()
        {
        }
        #endregion

        #region Properties
        public static StoneDataSet DataSet { get; set; }
        #endregion

        #region Methods
        public static int GenerateDataSet(string filePath)
        {
            DataSet = new StoneDataSet();
            int tuplesCount = DataSet.GenerateTuples();
            DataSet.SaveToFile(filePath);
            return tuplesCount;
        }
        public static void Run()
        {
            AxesBuilder.Origin.Frequency = ApplicationSettings.Instance.OriginFrequency;
            AxesBuilder.Origin.LifeTime = ApplicationSettings.Instance.OriginLifeTime;
            AxesBuilder.Origin.Weight = ApplicationSettings.Instance.OriginWeight;

            AxesBuilder.Clustres = new List<StoneCluster>();

            DCC.Groups = new StoneGroup[8];
            for (int i = 0; i < 8; i++)
                DCC.Groups[i] = new StoneGroup(i);

            DataAggregator.Initialize();

            DataSet = new StoneDataSet();
            DataSet.LoadFromFile(ApplicationSettings.Instance.LoadFilePath);

            PersistThread.Start();
            RunStone(DataSet);

            ApplicationSettings.Instance.OriginFrequency = AxesBuilder.Origin.Frequency;
            ApplicationSettings.Instance.OriginLifeTime = AxesBuilder.Origin.LifeTime;
            ApplicationSettings.Instance.OriginWeight = AxesBuilder.Origin.Weight;

            ApplicationSettings.Instance.Save();
        }

        private static void RunStone(StoneDataSet DataSet)
        {
            if (DataSet == null || DataSet.Tuples.Count == 0)
                throw new HelperClasses.LibraryException("DataSet از فایل خوانده نشده است.");

            foreach (DataSetTuple dataSetTuple in DataSet.Tuples)
            {
                DataAggregator.AddTuple(dataSetTuple);
                FilterComponent.AddTuple(dataSetTuple);
            }
        }
        public static int LoadDataSet(string filePath)
        {
            DataSet = new StoneDataSet();
            return DataSet.LoadFromFile(filePath);
        }
        public static void Persist()
        {
            var db = DB.NewContext;
            DCC.Persist(db);
            AxesBuilder.Persist(db);
            db.SaveChanges();
        }
        public static void Refresh()
        {
            OfflineDB.RefreshFromDB();
            ProfileMaintainer.ComputeDayWeights();
            DCC.Refresh();
            AxesBuilder.UpdateOrigin();
            ProfileMaintainer.Refresh();
        }
        #endregion
    }
}

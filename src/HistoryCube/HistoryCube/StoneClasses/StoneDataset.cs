using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HistoryCube
{
    public class StoneDataSet
    {
        public List<DataSetTuple> Tuples { get; set; }
        public int GenerateTuples()
        {
            Random randomNumberGenerator = new Random();
            double referenceTime = 0;
            double nextTime = referenceTime;
            int maxTransactionTime = ApplicationSettings.Instance.MaxTransactionTime;
            int maxStep = ApplicationSettings.Instance.MaxStep;
            string destinationIp = ApplicationSettings.Instance.DestinationIp;
            this.Tuples = new List<DataSetTuple>();
            for (int i = 0; i < ApplicationSettings.Instance.DatasetTuplesCount; i++)
            {
                int packetsCount = randomNumberGenerator.Next(100);
                int bytesCount = packetsCount * 10;
                double startTime = nextTime;
                double endTime = startTime+(randomNumberGenerator.Next(maxTransactionTime));
                nextTime = nextTime+(randomNumberGenerator.Next(maxStep));
                this.Tuples.Add(new DataSetTuple(IPGenerator.Generate(), destinationIp, packetsCount, bytesCount, startTime, endTime));
            }
            return this.Tuples.Count;
        }
        public void SaveToFile(string filePath)
        {
            File.WriteAllText(filePath,"");
            foreach (DataSetTuple t in this.Tuples)
                File.AppendAllText(filePath, t.ToString() + "\r\n");
        }
        public int LoadFromFile(string filePath)
        {
            this.Tuples = new List<DataSetTuple>();
            var lines=File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                DataSetTuple tuple = new DataSetTuple(line);
                this.Tuples.Add(tuple);
            }
            return this.Tuples.Count;
        }
    }
}

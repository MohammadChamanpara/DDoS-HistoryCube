using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class DataSetConverter
    {

        public static int Convert(string inputPath, string outputPath, string reportPath)
        {
            int errorCount = 0;
            var windowInputTuples = new List<ConvertInputTuple>();
            var allOutputTuples = new List<DataSetTuple>();
            var errorInputTuples = new List<ConvertInputTuple>();

            var inputLines = File.ReadAllLines(inputPath);
            double windowLength = ApplicationSettings.Instance.ConvertWindowLength;
            double lastTimeStamp = 0;
            int lineNumber = 0;
            int linesCount = inputLines.Count();
            foreach (string inputLine in inputLines)
            {
                var inputTuple = new ConvertInputTuple(inputLine, ++lineNumber);
                windowInputTuples.Add(inputTuple);
                if (!inputTuple.IsValid)
                {
                    errorCount++;
                    errorInputTuples.Add(inputTuple);
                    continue;
                }
                if (lastTimeStamp == 0)
                    lastTimeStamp = inputTuple.TimeStamp;

                if (inputTuple.TimeStamp - lastTimeStamp < windowLength && lineNumber < linesCount)
                    continue;

                var sourceGroups = windowInputTuples.GroupBy(x => x.SourceIp);
                foreach (var sourceGroup in sourceGroups)
                {
                    var destinationGroups = sourceGroup.GroupBy(x => x.DestinationIp);
                    foreach (var destinationGroup in destinationGroups)
                    {
                        allOutputTuples.Add
                        (
                            new DataSetTuple
                            (
                                sourceGroup.First().SourceIp, 
                                destinationGroup.First().DestinationIp, 
                                destinationGroup.Count(), 
                                destinationGroup.Sum(x => x.BytesCount), 
                                lastTimeStamp, inputTuple.TimeStamp
                             )
                        );
                    }
                }
                lastTimeStamp = inputTuple.TimeStamp;
                windowInputTuples.Clear();
            }

            var outputLines = new List<string>();
            allOutputTuples.ForEach(x => outputLines.Add(x.ToString()));
            File.WriteAllLines(outputPath, outputLines);


            File.WriteAllText(reportPath, "Conversion Report. < " + DateTime.Now.ToString() + " >\r\n");

            if (errorInputTuples.Count == 0)
                File.AppendAllText(reportPath, "Dataset Converted Successfully.");
            else
            {
                var errorLines = new List<string>();
                errorInputTuples.ForEach(x => errorLines.Add(x.ToString()));
                File.AppendAllLines(reportPath, errorLines);
            }
            return errorCount;
        }

        public static int PacketCount { get; set; }
    }
}

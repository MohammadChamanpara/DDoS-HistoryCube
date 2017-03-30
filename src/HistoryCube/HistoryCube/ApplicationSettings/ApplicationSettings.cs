using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.Ports;
using Library;
using System.Reflection;
using System.IO;
using Library.Settings;

namespace HistoryCube
{
    public class ApplicationSettings
    {
        public static ApplicationSettingsInstance Instance = new ApplicationSettingsInstance();
        public class ApplicationSettingsInstance : ApplicationSettingsInstanceBase
        {
            [Category("Generate Dataset")]
            [SettingProperty(1000)]
            [DisplayName("Dataset Tuples Count")]
            public int DatasetTuplesCount { get; set; }

            [Category("Generate Dataset")]
            [DisplayName("Generate Tuple Separator")]
            public char GenerateTupleSeparator { get { return TupleSeparator; } set { TupleSeparator = value; } }

            [Category("Input Dataset")]
            [DisplayName("Tuple Separator")]
            [SettingProperty(',')]
            public char TupleSeparator { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Source Ip Position")]
            [SettingProperty(0)]
            public int SourceIpPosition { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Destination Ip Position")]
            [SettingProperty(1)]
            public int DestinationIpPosition { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Packets Count Position")]
            [SettingProperty(2)]
            public int PacketsCountPosition { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Bytes Count Position")]
            [SettingProperty(3)]
            public int BytesCountPosition { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Start Time Position")]
            [SettingProperty(4)]
            public int StartTimePosition { get; set; }

            [Category("Input Dataset")]
            [DisplayName("EndTime Position")]
            [SettingProperty(5)]
            public int EndTimePosition { get; set; }

            [Category("Generate Dataset")]
            [DisplayName("Max Transaction Time")]
            [SettingProperty(60 * 60 * 2)]
            public int MaxTransactionTime { get; set; }

            [Category("Generate Dataset")]
            [DisplayName("Max Step")]
            [SettingProperty(5)]
            public int MaxStep { get; set; }

            [Category("Generate Dataset")]
            [DisplayName("Destination Ip")]
            [SettingProperty("1.1.1.1")]
            public string DestinationIp { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Generate FilePath")]
            [SettingProperty("d:\\GenerateDataSet.txt")]
            public string GenerateFilePath { get; set; }

            [Category("Input Dataset")]
            [DisplayName("Load FilePath")]
            [SettingProperty("d:\\LoadDataSet.txt")]
            public string LoadFilePath { get; set; }

            [Category("Window")]
            [DisplayName("Window Size")]
            [SettingProperty(60)]
            public int WindowSize { get; set; }

            [Category("Window")]
            [DisplayName("Window Advance")]
            [SettingProperty(10)]
            public int WindowAdvance { get; set; }


            [Category("DCC")]
            [DisplayName("Alarm Tolerance")]
            [SettingProperty(10)]
            public int AlarmTolerance { get; set; }

            [Category("DCC")]
            [DisplayName("History Days Count")]
            [SettingProperty(12)]
            public int HistoryDaysCount { get; set; }

            [Category("DCC")]
            [DisplayName("Historic Weight")]
            [SettingProperty(2.0)]
            public double HistoricWeight { get; set; }

            [Category("Axes Builder")]
            [DisplayName("Origin Frequency")]
            [SettingProperty(1)]
            public int OriginFrequency { get; set; }

            [Category("Axes Builder")]
            [DisplayName("Origin LifeTime")]
            [SettingProperty(1)]
            public int OriginLifeTime { get; set; }

            [Category("Axes Builder")]
            [DisplayName("Origin Weight")]
            [SettingProperty(1)]
            public int OriginWeight { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Window Length")]
            [SettingProperty(2.0)]
            public double ConvertWindowLength { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Position-Destination Ip")]
            [SettingProperty(5)]
            public int ConvertDestinationIpPosition { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Position-Source Ip")]
            [SettingProperty(6)]
            public int ConvertSourceIpPosition { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Position-Bytes Count")]
            [SettingProperty(3)]
            public int ConvertBytesCountPosition { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Position-TimeStamp")]
            [SettingProperty(0)]
            public int ConvertTimeStampPosition { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Convert Input file Tuple Separator")]
            [SettingProperty(' ')]
            public char ConvertInputFileTupleSeparator { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Convert From Path ")]
            [SettingProperty("")]
            public string ConvertFromPath { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Convert To Path ")]
            [SettingProperty("")]
            public string ConvertToPath { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Convert Report Path ")]
            [SettingProperty("")]
            public string ConvertReportPath { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Use Default Bytes Count")]
            [SettingProperty(true)]
            public bool ConvertUseDefaultBytesCount { get; set; }

            [Category("Convert DataSet")]
            [DisplayName("Default Bytes Count")]
            [SettingProperty(200)]
            public int ConvertDefaultBytesCount { get; set; }

            [Category("FilterComponent")]
            [DisplayName("Known Clusters Capacity")]
            [SettingProperty(0.95)]
            public double KnownCapacity { get; set; }

            [Category("FilterComponent")]
            [DisplayName("Filter Component Is Active")]
            [SettingProperty(true)]
            public bool FilterComponentIsActive { get; set; }

            [Category("FilterComponent")]
            [DisplayName("Filter Component Server Input Link File")]
            [SettingProperty(@"C:\Stone\Output\ServerInputLinkFile.txt")]
            public string ServerInputLinkFile { get; set; }

            [Category("HistoryCube")]
            [DisplayName("Persist Interval in Seconds")]
            [SettingProperty(30)]
            public int PersistThreadInterval { get; set; }
        }

    }
}

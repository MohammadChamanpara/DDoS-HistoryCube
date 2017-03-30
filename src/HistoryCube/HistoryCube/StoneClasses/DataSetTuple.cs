using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library;
using Library.Log;

namespace HistoryCube
{
    public class DataSetTuple
    {
        #region Properties
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public int PacketsCount { get; set; }
        public int BytesCount { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public StoneFeatures StoneFeatures
        {
            get
            {
                return new StoneFeatures()
                {
                    Frequency = this.PacketsCount,
                    LifeTime = (int)(this.EndTime - this.StartTime),
                    Weight = this.BytesCount
                };
            }
        }
        private int _clusterPrefix = 0;
        public int ClusterPrefix
        {
            get
            {
                if (_clusterPrefix == 0)
                {
                    if (!this.SourceIp.HasValue())
                        _clusterPrefix = 0;
                    var parts = this.SourceIp.Split('.');
                    if (parts.Count() != 4)
                        throw HelperMethods.CreateException("فرمت آدرس مبدأ صحیح نمی باشد.");
                    if (!int.TryParse(parts[0], out _clusterPrefix))
                        throw HelperMethods.CreateException("فرمت آدرس مبدأ صحیح نمی باشد.");
                }
                return _clusterPrefix;
            }
        }
        #endregion

        #region Constructor
        public DataSetTuple(string sourceIp, string destinationIp, int packetsCount, int bytesCount, double startTime, double endTime)
        {
            this.SourceIp = sourceIp;
            this.DestinationIp = destinationIp;
            this.PacketsCount = packetsCount;
            this.BytesCount = bytesCount;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        public DataSetTuple(string stringTuple)
        {
            string[] tupleParts = stringTuple.Split(ApplicationSettings.Instance.TupleSeparator);
            string sourceIpString = tupleParts[ApplicationSettings.Instance.SourceIpPosition];
            string destinationIpString = tupleParts[ApplicationSettings.Instance.DestinationIpPosition];
            string packetsCountString = tupleParts[ApplicationSettings.Instance.PacketsCountPosition];
            string bytesCountString = tupleParts[ApplicationSettings.Instance.BytesCountPosition];
            string startTimeString = tupleParts[ApplicationSettings.Instance.StartTimePosition];
            string endTimeString = tupleParts[ApplicationSettings.Instance.EndTimePosition];

            this.SourceIp = sourceIpString;
            int dotCount = sourceIpString.Count(x => x == '.');
            if (dotCount != 3)
                LogService.LogError("IP فرستنده از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به IP نمی باشد.".FormatWith(packetsCountString));


            this.DestinationIp = destinationIpString;
            dotCount = sourceIpString.Count(x => x == '.');
            if (dotCount != 3)
                throw HelperMethods.CreateException("IPگیرنده از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به IP نمی باشد.", packetsCountString);

            int packetCount;
            var result = int.TryParse(packetsCountString, out packetCount);
            if (result == false)
                throw HelperMethods.CreateException("تعداد پکت ها از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به عدد صحیح نمی باشد.", packetsCountString);

            this.PacketsCount = packetCount;

            int bytesCount;
            result = int.TryParse(bytesCountString, out bytesCount);
            if (result == false)
                throw HelperMethods.CreateException("تعداد بایت ها از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به عدد صحیح نمی باشد.", packetsCountString);
            this.BytesCount = bytesCount;

            double dateTime;
            if (double.TryParse(startTimeString, out dateTime))
                this.StartTime = dateTime;
            else
                throw HelperMethods.CreateException("زمان شروع از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به عدد اعشاری نمی باشد.", startTimeString);

            if (double.TryParse(endTimeString, out dateTime))
                this.EndTime = dateTime;
            else
                throw HelperMethods.CreateException("زمان خاتمه از تاپل قابل بازیابی نیست. رشته {0} قابل تبدیل به تاریخ نمی باشد.", endTimeString);
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return string.Join
            (
                ApplicationSettings.Instance.TupleSeparator.ToString(),
                this.SourceIp,
                this.DestinationIp,
                this.PacketsCount.ToString(),
                this.BytesCount.ToString(),
                this.StartTime.ToString(),
                this.EndTime.ToString()
             );
        }
        #endregion
    }
}

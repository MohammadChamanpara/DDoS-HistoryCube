using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class ConvertInputTuple
    {

        public ConvertInputTuple(string inputLine, int lineNumber)
        {
            this.IsValid = false;

            this.InputString = inputLine;
            this.LineNumber = lineNumber;

            var parts = inputLine.Split(ApplicationSettings.Instance.ConvertInputFileTupleSeparator);
            if (parts.Count() <= ApplicationSettings.Instance.ConvertSourceIpPosition)
            {
                this.Error = "InputTuple parts count is less than specified number.";
                return;
            }
            var timeStampString = parts[ApplicationSettings.Instance.ConvertTimeStampPosition];
            var bytesCountString = parts[ApplicationSettings.Instance.ConvertBytesCountPosition];
            var sourceIpString = parts[ApplicationSettings.Instance.ConvertSourceIpPosition];
            var destinationIpString = parts[ApplicationSettings.Instance.ConvertDestinationIpPosition];

            double doubleResult;
            if (double.TryParse(timeStampString, out doubleResult))
                this.TimeStamp = doubleResult;
            else
            {
                this.Error = string.Format("Value {0} cannot be converted to a time stamp.", timeStampString);
                return;
            }

            int result;
            if (int.TryParse(bytesCountString, out result))
                this.BytesCount = result;
            else
            {
                if (ApplicationSettings.Instance.ConvertUseDefaultBytesCount)
                    this.BytesCount = ApplicationSettings.Instance.ConvertDefaultBytesCount;
                else
                {
                    this.Error = string.Format("Value {0} cannot be converted to a bytes count.", bytesCountString);
                    return;
                }
            }

            if (sourceIpString.Split('.').Count() == 4)
                this.SourceIp = sourceIpString;
            else
            {
                this.Error = string.Format("Value {0} cannot be converted to source Ip.", sourceIpString);
                return;
            }

            this.DestinationIp = destinationIpString;
            this.IsValid = true;
        }
        public int BytesCount { get; set; }

        public bool IsValid { get; set; }

        public double TimeStamp { get; set; }

        public string SourceIp { get; set; }

        public string DestinationIp { get; set; }

        public string Error { get; set; }

        public string InputString { get; set; }

        public int LineNumber { get; set; }

        public override string ToString()
        {
            if (this.IsValid)
                return this.InputString;
            else
                return "<INVALID>\r\nLineNumber : " + this.LineNumber + "\r\nTuple : " + this.InputString + "\r\nError : " + this.Error + "\r\n--------------------------------";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class AlarmException : Exception
    {
        private int Group { get; set; }
        private double Rc { get; set; }
        private double Re { get; set; }
        private int Tolerance { get; set; }
        public AlarmException(int group, double rc, double re, int tolerance)
            : base
            (
                string.Format
                (
                    "Attack Alarm. \r\n\r\nGroup : {0}\r\nCurrent Count:{1}\r\nHistory Average : {2}\r\nTolerance : {3}",
                    group, rc, re, tolerance
                )
            )
        {
            this.Group = group;
            this.Rc = rc;
            this.Re = re;
            this.Tolerance = tolerance;
        }
    }
}

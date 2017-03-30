using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class StoneFeatures
    {
        public int Frequency { get; set; }

        public int Weight { get; set; }

        public int LifeTime { get; set; }
        public Boolean HasValue
        {
            get
            {
                return (LifeTime != 0 || this.Weight != 0 || this.Frequency != 0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class StoneSuspiciousChanel:StoneBaseChanel
    {
        public override double Capacity
        {
            get
            {
                return 1- ApplicationSettings.Instance.KnownCapacity ;
            }
        }


  
    }
}

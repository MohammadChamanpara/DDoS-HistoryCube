using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class StoneGroupChanel : StoneBaseChanel
    {
        public StoneGroup Group { get; set; }
        public StoneGroupChanel(StoneGroup group)
        {
            this.Group = group;
        }
        public override double Capacity
        {
            get
            {
                return ApplicationSettings.Instance.KnownCapacity * this.Group.Weight;
            }
        }
    }
}

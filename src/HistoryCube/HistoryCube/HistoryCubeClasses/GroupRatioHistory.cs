﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube.Model
{
    public partial class GroupRatioHistory
    {
        public int ToBinaryValue()
        {
            return BinaryMemory.ConcatNumbers(GroupNumber, Day, Ratio);
        }
        public void FromBinaryValue(int binaryValue)
        {
            int groupNumber;
            int day;
            int value;
            BinaryMemory.SplitNumbers(binaryValue, out groupNumber, out day, out value);
            this.GroupNumber = groupNumber;
            this.Day = day;
            this.Ratio = value;
        }


    }
}

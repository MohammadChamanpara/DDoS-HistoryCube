using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public class BinaryMemory
    {
        #region Constants
        const int PartitionCapacity = sizeof(ulong) * 8;
        #endregion

        #region Properties
        List<ulong> Partitions { get; set; }
        #endregion

        #region Constructor
        public BinaryMemory()
        {
            this.Partitions = new List<ulong>();
        }
        #endregion

        #region Methods
        public void Add(int item)
        {
            int partitionNumber = GetPartitionNumber(item);
            while (Partitions.Count <= partitionNumber)
                Partitions.Add(0);
            int indexInPartition = GetIndexInPartition(item);
            Partitions[partitionNumber] += (ulong)Math.Pow(2, indexInPartition);
        }

        private static int GetIndexInPartition(int item)
        {
            return (int)(item % PartitionCapacity);
        }

        private int GetPartitionNumber(int item)
        {
            return (int)(item / PartitionCapacity);
        }

        public Boolean Contains(int item)
        {
            int indexInPartition = GetIndexInPartition(item);
            int partitionNumber = GetPartitionNumber(item);
            if (this.Partitions.Count <= partitionNumber)
                return false;
            int binaryValue = (int)Math.Pow(2, indexInPartition);
            return ((Partitions[partitionNumber] & (ulong)binaryValue) == (ulong)binaryValue);
        }
        public List<int> RetrieveItems()
        {
            List<int> result = new List<int>();
            int retrievedItem = 0;
            foreach (var partition in Partitions)
            {
                ulong bitValue = 1;
                for (int i = 0; i < PartitionCapacity; i++)
                {
                    if ((partition & bitValue) == bitValue)
                    {
                        result.Add(retrievedItem);
                    }
                    bitValue *= 2;
                    retrievedItem++;
                }
            }
            return result;
        }

        public static int ConcatNumbers(int groupNumber, int day, int value)
        {
            string groupString = (groupNumber + 1).ToString();
            string dayString = day.ToString();
            if (day < 10)
                dayString = "0" + dayString;

            string concat = groupString + dayString + value.ToString();
            return int.Parse(concat);
        }
        public static void SplitNumbers(int binaryValue, out int groupNumber, out int day, out int value)
        {
            string groupString = binaryValue.ToString().Substring(0, 1);
            groupNumber = int.Parse(groupString) - 1;

            string dayString = binaryValue.ToString().Substring(1, 2);
            day = int.Parse(dayString);

            value = int.Parse(binaryValue.ToString().Substring(3));
        }
        #endregion
    }
}
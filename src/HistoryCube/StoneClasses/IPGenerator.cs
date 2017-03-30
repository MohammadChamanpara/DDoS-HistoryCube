using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HistoryCube
{
    public static class IPGenerator
    {
        public static string Generate()
        {
            var seedGenerator=new Random();
            var octetGenerator=new Random(seedGenerator.Next(1000));
            int octet1 = octetGenerator.Next(4);
            int octet2 = octetGenerator.Next(4);
            int octet3 = octetGenerator.Next(4);
            int octet4 = octetGenerator.Next(4);
            return string.Join(".", octet1.ToString(), octet2.ToString(), octet3.ToString(), octet4.ToString());
        }
    }
}

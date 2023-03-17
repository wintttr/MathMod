using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffEquation
{
    public class Range
    {
        public Range(double a, double b)
        {
            if (a > b)
                (a, b) = (b, a);

            this.Start = a;
            this.End = b;
        }

        public double Diff
        {
            get
            {
                return End - Start;
            }
        }

        public double Start { get; set; }
        public double End { get; set; }
    }
}

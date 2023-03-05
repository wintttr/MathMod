using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace RandomTest
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

        public override int GetHashCode()
        {
            return ((int) Start) ^ ((int) End);
        }

        override public bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            Range r = (Range) obj;
            return this.Start == r.Start && this.End == r.End;
        }

        public double Start { get; set; }
        public double End { get; set; }
    }
}

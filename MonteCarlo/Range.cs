using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    class Range
    {
        public Range(double a, double b)
        {
            if (a > b)
                (a, b) = (b, a);

            this.a = a;
            this.b = b;
        }

        public double Diff
        {
            get
            {
                return b - a;
            }
        }

        public double a { get; set; }
        public double b { get; set; }
    }
}

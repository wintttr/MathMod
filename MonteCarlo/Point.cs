using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class Point
    {
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        static double RandomDouble(double a, double b, Random rand)
        {
            return a + rand.NextDouble() * (b - a);
        }

        static double RandomDouble(Range range, Random rand)
        {
            return RandomDouble(range.Start, range.End, rand);
        }

        static public Point RandomPoint(Range x_range, Range y_range, Random rand)
        {
            return new Point(RandomDouble(x_range, rand), RandomDouble(y_range, rand));
        }


        public double x { get; set; }
        public double y { get; set; }
    }
}

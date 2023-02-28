using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class Task3 : BasicTask
    {
        const double variant = Constants.Var;
        const double eps = Constants.Eps;

        public double ApprPI
        {
            get
            {
                return ApproximateSquare() / Math.Pow(variant, 2);
            }
        }

        public override string TaskTitle
        {
            get
            {
                return "Вычисление числа ПИ";
            }
        }

        static List<DataPoint> GetCirclePoints()
        {
            List<DataPoint> DP = new();

            for (double rad = 0; rad <= Math.PI * 2; rad += eps)
            {
                DP.Add(new DataPoint(variant * Math.Cos(rad), variant * Math.Sin(rad)));
            }

            return DP;
        }

        public override void AddFuncs(MainViewModel model)
        {
            model.AddLineSeries(GetCirclePoints());
        }

        public override bool isInFigure(Point p)
        {
            return Math.Pow(p.x, 2) + Math.Pow(p.y, 2) < Math.Pow(variant, 2);
        }

        public override Range GetXRange()
        {
            return new(-variant, variant);
        }

        public override Range GetYRange()
        {
            return new(-variant, variant);
        }

        override public double ExactSquare()
        {
            return Math.PI * Math.Pow(variant, 2);
        }

        public override List<String> AdditiveCalcs()
        {
            return new()
            {
                String.Format("PI = {0:F4}", ApprPI)
            };
        }

        public override List<String> AdditiveErrors()
        {
            double absolute = Math.Abs(Math.PI - ApprPI);
            double relative = absolute / ApprPI * 100;
            return new()
            {
                String.Format("Аболютная погрешность PI: {0:F4}", absolute),
                String.Format("Относительная погрешность PI: {0:F4}%", relative)
            };
        }
    }
}

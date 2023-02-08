using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinQ
{
    class Compute
    {
        
        public static (double, double) LinearInterp(ICollection<double> Xs, ICollection<double> Ys)
        {
            int n = Xs.Count;
            double x_sum = Xs.Sum();
            double y_sum = Ys.Sum();
            double x_2_sum = (from x in Xs select x * x).Sum();
            double x_y_sum = (from (double x, double y) Cor in Xs.Zip(Ys) select Cor.x * Cor.y).Sum();

            double determinant = x_2_sum * n - x_sum * x_sum;
            double d_a = x_y_sum * n - x_sum * y_sum;
            double d_b = x_2_sum * y_sum - x_y_sum * x_sum;

            return (d_a / determinant, d_b / determinant);
        }

        public static (double, double) PowerInterp(ICollection<double> xs, ICollection<double> ys)
        {
            List<double> Xs = new(from x in xs select Math.Log(x));
            List<double> Ys = new(from y in ys select Math.Log(y));

            (double a, double b) = LinearInterp(Xs, Ys);

            return (a, Math.Exp(b));
        }

        public static (double, double) ExponentialInterp(ICollection<double> Xs, ICollection<double> ys)
        {
            List<double> Ys = new(from y in ys select Math.Log(y));

            (double a, double b) = LinearInterp(Xs, Ys);

            return (a, Math.Exp(b));
        }

        static double Det3(double[,] m)
        {
            return m[0, 0] * m[1, 1] * m[2, 2] + m[1, 0] * m[0, 2] * m[2, 1] + m[0, 1] * m[2, 0] * m[1, 2]
                 - m[2, 0] * m[1, 1] * m[0, 2] - m[0, 1] * m[1, 0] * m[2, 2] - m[0, 0] * m[1, 2] * m[2, 1];
        }

        static void SwapCols(double[,] m, int a, int b)
        {
            if (a >= m.GetLength(1) || b >= m.GetLength(1))
                throw new Exception("Номер колонки больше, чем число колонок.");

            for(int i = 0; i < m.GetLength(0); i++)
            {
                (m[i, a], m[i, b]) = (m[i, b], m[i, a]);
            }
        }

        public static (double, double, double) QuadraticInterp(ICollection<double> Xs, ICollection<double> Ys)
        {

            double n = Xs.Count;
            double x_1 = Xs.Sum();
            double x_2 = (from x in Xs select Math.Pow(x, 2)).Sum();
            double x_3 = (from x in Xs select Math.Pow(x, 3)).Sum();
            double x_4 = (from x in Xs select Math.Pow(x, 4)).Sum();

            double y_1 = Ys.Sum();
            double x_y = (from (double x, double y) Cor in Xs.Zip(Ys) select Cor.x * Cor.y).Sum();
            double x_2_y = (from (double x, double y) Cor in Xs.Zip(Ys) select Math.Pow(Cor.x, 2) * Cor.y).Sum();

            double[,] matr = new double[3, 4];

            matr[0, 0] = x_4;
            matr[0, 1] = x_3;
            matr[0, 2] = x_2;
            matr[0, 3] = x_2_y;

            matr[1, 0] = x_3;
            matr[1, 1] = x_2;
            matr[1, 2] = x_1;
            matr[1, 3] = x_y;

            matr[2, 0] = x_2;
            matr[2, 1] = x_1;
            matr[2, 2] = n;
            matr[2, 3] = y_1;

            double determinant = Det3(matr);
            double[,] temp = (double[,]) matr.Clone();
            
            SwapCols(temp, 0, 3);
            double d_a = Det3(temp);
            SwapCols(temp, 0, 3);

            SwapCols(temp, 1, 3);
            double d_b = Det3(temp);
            SwapCols(temp, 1, 3);

            SwapCols(temp, 2, 3);
            double d_c = Det3(temp);
            SwapCols(temp, 2, 3);

            double a = d_a / determinant;
            double b = d_b / determinant;
            double c = d_c / determinant;

            return (a, b, c);
        }
        public static double Linear(double x, double a, double b)
        {
            return a * x + b;
        }

        public static double Power(double x, double a, double b)
        {
            return b * Math.Pow(x, a);
        }

        public static double Exponential(double x, double a, double b)
        {
            return b * Math.Exp(a * x);
        }

        public static double Quadratic(double x, double a, double b, double c)
        {
            return a * Math.Pow(x, 2) + b * x + c;
        }
    }
}
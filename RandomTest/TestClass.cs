using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RandomTest
{
    public struct TaggedRange
    {
        public TaggedRange(Range r, double v)
        {
            Range = r;
            Value = v;
        }

        public Range Range { get; private set; }
        public double Value { get; private set; }
    }

    public class TestClass
    {
        static public List<double> GenerateRandomList(int n, Func<double> randFunc)
        {
            return new(from _ in Enumerable.Range(0, n) select randFunc());
        }

        static public List<TaggedRange> GetFreqList(ICollection<double> randomNumbers, int k)
        {
            const double b = 1;

            Dictionary<Range, int> rangeCount = new();

            for (int i = 1; i <= k; i++)
            {
                rangeCount.Add(new(b / k * (i - 1), b / k * i), 0);
            }

            foreach (var num in randomNumbers)
            {
                int ind = (int)Math.Truncate(num / (b / k));
                Range r = new(b / k * ind, b / k * (ind + 1));
                rangeCount[r]++;
            }

            return new(from range in rangeCount.Keys select new TaggedRange(range, rangeCount[range]));
        }


        static public double MathExpectation(ICollection<double> numbers)
        {
            return numbers.Average();
        }

        static public double Variance(ICollection<double> numbers, double? mathExp = null)
        {
            double mExp = mathExp ?? MathExpectation(numbers);

            return (from p in numbers select Math.Pow(p - mExp, 2)).Average();
        }

        static public int FreqTest(ICollection<double> randomNumbers)
        {
            return randomNumbers
                    .Where(x => x >= Constants.M - Constants.Sigma && x <= Constants.M + Constants.Sigma)
                    .Count();
        }

        static public double ChiSquareTest(ICollection<double> randomNumbers, int k)
        {
            List<TaggedRange> lr = GetFreqList(randomNumbers, k);
            int n = randomNumbers.Count;
            double p = 1.0 / (double)k;
            double practicalChi = (from num in lr select Math.Pow(num.Value - p*n, 2) / (p * n)).Sum();
            return practicalChi;
        }

        static public double ChiSquareProbability(double chi, int n)
        {
            Dictionary<int, List<double>> theoreticChi = new()
            {
                { -1, new() { 1, 5, 25, 50, 75, 95, 99} },
                { 1, new() {0.00016, 0.00393, 0.1015, 0.4549, 1.323, 3.841, 6.635} },
                { 2, new() {0.02010, 0.1026, 0.5754, 1.386, 2.773, 5.991, 9.210} },
                { 3, new() {0.1148, 0.3518, 1.213, 2.366, 4.108, 7.815, 11.34} },
                { 4, new() {0.2971, 0.7107, 1.923, 3.357, 5.385, 9.488, 13.28} },
                { 5, new() {0.5543, 1.1455, 2.675, 4.351, 6.626, 11.07, 15.09} },
                { 6, new() {0.8721, 1.635, 3.455, 5.348, 7.841, 12.59, 16.81} },
                { 7, new() {1.239, 2.167, 4.255, 6.346, 9.037, 14.07, 18.48}},
                { 8, new() {1.646, 2.733, 5.071, 7.344, 10.22, 15.51, 20.09} },
                { 9, new() {2.088, 3.325, 5.899, 8.343, 11.39, 16.92, 21.67} },
                { 10, new() {2.558, 3.940, 6.737, 9.342, 12.55, 18.31, 23.21} },
                { 11, new() {3.053, 4.575, 7.584, 10.34, 13.70, 19.68, 24.72} },
                { 12, new() {3.571, 5.226, 8.438, 11.34, 14.85, 21.03, 26.22} },
                { 15, new() {5.229, 7.261, 11.04, 14.34, 18.25, 25.00, 30.58} },
                { 20, new() {8.260, 10.85, 15.45, 19.34, 23.83, 31.41, 37.57} },
                { 30, new() {14.95, 18.49, 24.48, 29.34, 34.80, 43.77, 50.89} },
                { 50, new() {29.71, 34.76, 42.94, 49.33, 56.33, 67.50, 76.15} },
            };

            int v = n - 1;
            List<double> curTheorChi;
            if (v <= 50) {
                v = theoreticChi.Keys.Where(x => x <= v).Max();
                curTheorChi = theoreticChi[v];
            }
            else
            {
                List<double> prob = new() { -2.33, -1.64, -0.674, 0, 0.674, 1.64, 2.33};
                curTheorChi = new(from p in prob select v + Math.Sqrt(2*v)*p + 2.0 / 3.0*Math.Pow(p,2) - 2.0 / 3.0 + 1 / Math.Sqrt(v));
            }

            for (int i = 0, j = 1; j < curTheorChi.Count; i++, j++)
                if (chi >= curTheorChi[i] && chi <= curTheorChi[j])
                    return theoreticChi[-1][i];

            if (chi > curTheorChi.Max())
                return 100;
            else
                return 0;
        }
    }
}

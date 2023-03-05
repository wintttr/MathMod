using MVM;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace RandomTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new();
        MainViewModel model = new("");

        public MainWindow()
        {
            InitializeComponent();

            model.AddHistogramSeries("random");
            plot.Model = model.Model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int k = Int32.Parse(segmentSplit.Text);
                int n = Int32.Parse(numbersCount.Text);

                List<double> nums = TestClass.GenerateRandomList(n, rand.NextDouble);

                List<HistogramItem> histList = new();
                foreach (TaggedRange h in TestClass.GetFreqList(nums, k))
                    histList.Add(new HistogramItem(h.Range.Start, h.Range.End, h.Value * h.Range.Diff, 0));

                model.UpdateHistogram("random", histList);

                List<String> calcs = new();

                double M = TestClass.MathExpectation(nums);
                double D = TestClass.Variance(nums, M);
                double F = TestClass.FreqTest(nums);
                double Xp = TestClass.ChiSquareTest(nums, k);

                calcs.Add(String.Format("Мат. ожидание: {0:F4}", M));
                calcs.Add(String.Format("Дисперсия: {0:F4}", D));
                calcs.Add(String.Format("Частотный тест: {0:F4} ({1:F4}%)", F, F / (double) n * 100));
                calcs.Add(String.Format("Хи квадрат: {0:F4}", Xp));
                calcs.Add(String.Format("Хи квадрат: {0:F4}%", TestClass.ChiSquareProbability(Xp, n)));

                calcList.Children.Clear();
                foreach (var s in calcs)
                {
                    TextBlock t = new();
                    t.Text = s;
                    calcList.Children.Add(t);
                }

                plot.InvalidatePlot();
            }
            catch (FormatException)
            {
                MessageBox.Show("Типа обработал ошибку.");
            }
        }
    }
}

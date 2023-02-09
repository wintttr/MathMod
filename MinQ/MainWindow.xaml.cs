using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<double> X, Y;

        List<(String, double)> ErrorList = new();

        void checkbox_Checked(object sender, RoutedEventArgs ev)
        {
            var cb = (CheckBox)sender;

            Series serie = (Series)cb.Tag;

            serie.IsVisible = true;

            plot.InvalidatePlot(false);
        }

        void checkbox_Unchecked(object sender, RoutedEventArgs ev)
        {
            var cb = (CheckBox)sender;

            Series serie = (Series)cb.Tag;

            serie.IsVisible = false;

            plot.InvalidatePlot(false);
        }

        void AddCheckBox(String content, object tag)
        {
            var cb = new CheckBox { Content = content, Tag = tag, IsChecked = true };
            cb.Checked += (se, ev) => checkbox_Checked(se, ev);
            cb.Unchecked += (se, ev) => checkbox_Unchecked(se, ev);
            stackPanel.Children.Add(cb);
        }

        void AddListItem(String content)
        {
            listBox.Items.Add(content);
        }

        void AddCurve(Func<double, double> f, String title, MainViewModel m)
        {
            var s = m.AddFunc(f, title);

            if (s.Tag == null)
                throw new Exception("Тег почему-то нулёвый.........");

            double err = Compute.FindError(X, Y, (Func<double, double>)s.Tag);

            String content = String.Format("{0} ({1:F5})", title, err);
            AddCheckBox(content, s);

            ErrorList.Add((title, err));
        }

        void AddPoints(ICollection<double> X, ICollection<double> Y, String title, MainViewModel m)
        {
            List<ScatterPoint> l = new();

            foreach((double x, double y) in X.Zip(Y))
            {
                l.Add(new ScatterPoint(x, y, 4));
            }

            var s = m.AddScatterPoints(l, title);
            AddCheckBox(title, s);
        }

        public MainWindow()
        {
            InitializeComponent();

            X = new() { 10, 15, 20, 25, 30, 35 };
            Y = new() { 4.3, 3.3, 2.68, 2.25, 1.9, 1.7 };

            MainViewModel m = new MainViewModel("Метод наименьших квадратов", X.Min(), X.Max());

            (double a, double b) lin_coeff = Compute.LinearInterp(X, Y);
            (double a, double b) pow_coeff = Compute.PowerInterp(X, Y);
            (double a, double b) exp_coeff = Compute.ExponentialInterp(X, Y);
            (double a, double b, double c) quad_coeff = Compute.QuadraticInterp(X, Y);

            AddPoints(X, Y, "Original", m);

            AddCurve((x) => Compute.Linear(x, lin_coeff.a, lin_coeff.b), "Linear", m);
            AddListItem(String.Format("Linear (a = {0:F4}, b = {1:F4})", lin_coeff.a, lin_coeff.b));

            AddCurve((x) => Compute.Power(x, pow_coeff.a, pow_coeff.b), "Power", m);
            AddListItem(String.Format("Power (a = {0:F4}, b = {1:F4})", pow_coeff.a, pow_coeff.b));

            AddCurve((x) => Compute.Exponential(x, exp_coeff.a, exp_coeff.b), "Exponential", m);
            AddListItem(String.Format("Exponential (a = {0:F4}, b = {1:F4})", exp_coeff.a, exp_coeff.b));

            AddCurve((x) => Compute.Quadratic(x, quad_coeff.a, quad_coeff.b, quad_coeff.c), "Quadratic", m);
            AddListItem(String.Format("Quadratic (a = {0:F4}, b = {1:F4}, c = {2:F4})", quad_coeff.a, quad_coeff.b, quad_coeff.c));

            plot.Model = m.MyModel;
            
            closest.Content = String.Format("The closest func is {0}", ErrorList.MinBy((x) => x.Item2).Item1);
        }
    }
}
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
    /// 

    class Function
    {
        public Func<double, double> f { get; set; }
        public ICollection<(String, double)> attrs { get; set; }

        public Function(Func<double, double> f, ICollection<(String, double)> attrs)
        {
            this.f = f;
            this.attrs = attrs;
        }
    }
    
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

        void AddCurve(Function func, String title, MainViewModel m)
        {
            var s = m.AddFunc(func.f, title);

            if (s.Tag == null)
                throw new Exception("Тег почему-то нулёвый.........");

            double err = Compute.FindError(X, Y, (Func<double, double>)s.Tag);

            String checkbox_content = String.Format("{0} ({1:F5})", title, err);
            AddCheckBox(checkbox_content, s);

            String args = String.Empty;
            foreach ((String c, double d) in func.attrs)
                args += String.Format("{0} = {1:F4}, ", c, d);
            args = args.Trim();
            args = args.Trim(',');

            String listitem_content = String.Format("{0} ({1})", title, args);
            AddListItem(listitem_content);

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

            List<(String, double)> LinCoeffs = new() { ("a", lin_coeff.a), ("b", lin_coeff.b)};
            List<(String, double)> PowCoeffs = new() { ("a", pow_coeff.a), ("b", pow_coeff.b) };
            List<(String, double)> ExpCoeffs = new() { ("a", exp_coeff.a), ("b", exp_coeff.b) };
            List<(String, double)> QuadCoeffs = new() { ("a", quad_coeff.a), ("b", quad_coeff.b), ("c", quad_coeff.c) };

            AddPoints(X, Y, "Original", m);

            AddCurve(new Function((x) => Compute.Linear(x, lin_coeff.a, lin_coeff.b), LinCoeffs), "Linear", m);
            AddCurve(new Function((x) => Compute.Power(x, pow_coeff.a, pow_coeff.b), PowCoeffs), "Power", m);
            AddCurve(new Function((x) => Compute.Exponential(x, exp_coeff.a, exp_coeff.b), ExpCoeffs), "Exponential", m);
            AddCurve(new Function((x) => Compute.Quadratic(x, quad_coeff.a, quad_coeff.b, quad_coeff.c), QuadCoeffs), "Quadratic", m);

            plot.Model = m.MyModel;
            closest.Content = String.Format("The closest func is {0}", ErrorList.MinBy((x) => x.Item2).Item1);
        }
    }
}
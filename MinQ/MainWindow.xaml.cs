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
        Dictionary<String, int> i_s = new();
        void checkbox_Checked(object sender, RoutedEventArgs ev)
        {
            var view = (PlotView)this.FindName("plot");
            var series = view.Model.Series;

            var cb = (CheckBox)sender;

            series[i_s[cb.Name]].IsVisible = true;
            view.InvalidatePlot(false);
        }

        void checkbox_Unchecked(object sender, RoutedEventArgs ev)
        {
            var view = (PlotView)this.FindName("plot");
            var series = view.Model.Series;
            var cb = (CheckBox)sender;

            series[i_s[cb.Name]].IsVisible = false;
            view.InvalidatePlot(false);
        }

        public MainWindow()
        {
            InitializeComponent();

            var stackPanel = (StackPanel) this.FindName("stackPanel");

            PlotView a = new PlotView();
            
            var series = ((PlotView)this.FindName("plot")).Model.Series;

            for(int i = 0; i < series.Count; i++)
            {
                var cb = new CheckBox { Content = series[i].Title, Name = series[i].Title, IsChecked = true };
                cb.Checked += (se, ev) => this.checkbox_Checked(se, ev);
                cb.Unchecked += (se, ev) => this.checkbox_Unchecked(se, ev);
                stackPanel.Children.Add(cb);

                this.i_s[cb.Name] = i;
            }
        }
    }
}

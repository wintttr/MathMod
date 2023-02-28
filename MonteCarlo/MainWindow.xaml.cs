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
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonteCarlo
{
    public partial class MainWindow : Window
    {
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            BasicTaskWindow t = new(new Task1());
            t.ShowDialog();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            BasicTaskWindow t = new(new Task2());
            t.ShowDialog();
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            BasicTaskWindow t = new(new Task3());
            t.ShowDialog();
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            BasicTaskWindow t = new(new Task4());
            t.ShowDialog();
        }
    }
}
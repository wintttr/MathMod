using OxyPlot;
using OxyPlot.Series;
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
using System.Windows.Shapes;

namespace MonteCarlo
{
    /// <summary>
    /// Логика взаимодействия для BasicTaskWindow.xaml
    /// </summary>
    public partial class BasicTaskWindow : Window
    {
        Random rand = new();
        MainViewModel model;
        ScatterSeries innerScatterSeries, outerScatterSeries;
        BasicTask task;

        public BasicTaskWindow(BasicTask task)
        {
            InitializeComponent();

            this.task = task;

            this.Title = task.TaskTitle;

            model = new MainViewModel(task.TaskTitle);

            innerScatterSeries = model.AddScatterPoints();
            outerScatterSeries = model.AddScatterPoints();

            task.AddFuncs(model);

            plot.Model = model.MyModel;
        }

        static void AddToList(UIElementCollection l, List<String> col)
        {
            l.Clear();
            foreach (var s in col)
            {
                TextBlock err = new();
                err.Text = s;

                l.Add(err);
            }
        }

        protected void CalculateErrors(ICollection<Point> points)
        {
            List<String> errs = new(task.DefaultErrors());
            errs.AddRange(task.AdditiveErrors());

            AddToList(errorList.Children, errs);
        }

        protected void CalculateAddition(ICollection<Point> points)
        {
            List<String> calcs = new(task.DefaultCalcs());
            calcs.AddRange(task.AdditiveCalcs());
            
            AddToList(calcList.Children, calcs);
        }

        protected void generateButton_Click(object sender, RoutedEventArgs e)
        {
            int points_count;
            try
            {
                points_count = Int32.Parse(pointsCount.Text);

                List<Point> points = task.GeneratePoints(points_count, task.GetXRange(), task.GetYRange(), rand);

                List<ScatterPoint> innerPoints = new(), outerPoints = new();
                foreach (var p in points)
                {
                    if (task.isInFigure(p))
                        innerPoints.Add(new ScatterPoint(p.x, p.y));
                    else
                        outerPoints.Add(new ScatterPoint(p.x, p.y));
                }

                model.UpdateScatterPoints(innerPoints, innerScatterSeries);
                model.UpdateScatterPoints(outerPoints, outerScatterSeries);

                CalculateErrors(points);
                CalculateAddition(points);

                plot.Model = model.MyModel;
                plot.InvalidatePlot();
            }
            catch (FormatException)
            {
                MessageBox.Show("Число имеет неверный формат.");
            }
        }
    }
}

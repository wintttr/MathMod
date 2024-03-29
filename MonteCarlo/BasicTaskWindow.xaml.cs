﻿using OxyPlot;
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
        const double _pointSize = Constants.PointSize;

        Random rand = new();
        MainViewModel model;
        BasicTask task;

        public BasicTaskWindow(BasicTask task)
        {
            InitializeComponent();

            this.task = task;

            this.Title = task.TaskTitle;

            model = new MainViewModel(task.TaskTitle);


            var funcs = task.GetFuncs();
            foreach(List<Point> points in funcs)
                model.AddLineSeries(new List<DataPoint>(from p in points select new DataPoint(p.x, p.y)));
            

            model.AddScatterSeries("innerScatterSeries");
            model.AddScatterSeries("outerScatterSeries");

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
            List<String> errs = task.GetErrors(points);

            AddToList(errorList.Children, errs);
        }

        protected void CalculateAddition(ICollection<Point> points)
        {
            List<String> calcs = task.GetCalcs(points);
            
            AddToList(calcList.Children, calcs);
        }

        protected void generateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int points_count = Int32.Parse(pointsCount.Text);

                if(points_count < 0)
                {
                    throw new NegativeNumberException(points_count);
                }

                List<Point> points = BasicTask.GeneratePoints(points_count, task.GetXRange(), task.GetYRange(), rand);

                List<ScatterPoint> innerPoints = new(), outerPoints = new();
                foreach (var p in points)
                {
                    if (task.isInFigure(p))
                        innerPoints.Add(new ScatterPoint(p.x, p.y, _pointSize));
                    else
                        outerPoints.Add(new ScatterPoint(p.x, p.y, _pointSize));
                }

                model.UpdateScatterPoints(innerPoints, "innerScatterSeries");
                model.UpdateScatterPoints(outerPoints, "outerScatterSeries");

                CalculateErrors(points);
                CalculateAddition(points);

                plot.Model = model.MyModel;
                plot.InvalidatePlot();
            }
            catch (FormatException)
            {
                MessageBox.Show("Число имеет неверный формат.");
            }
            catch (NegativeNumberException ex)
            {
                MessageBox.Show(String.Format("Как я тебе {0} точек нарисую?", ex.number));
            }
            catch (EmptyListException)
            {
                MessageBox.Show(String.Format("Введите целое положительное число."));
            }
        }
    }
}

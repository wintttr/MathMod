using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVM
{
    public class MainViewModel
    {
        Dictionary<String, HistogramSeries> Histograms = new();

        public MainViewModel(String title)
        {
            Model = new PlotModel { Title = title };

            Model.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
            });
        }

        public HistogramSeries AddHistogramSeries(String tag)
        {
            HistogramSeries hs = new();
            hs.StrokeThickness = 1;
            Histograms.Add(tag, hs);

            Model.Series.Add(hs);
            return hs;
        }

        public void AddHistogramItems(String tag, ICollection<HistogramItem> items)
        {
            Histograms[tag].Items.AddRange(items);
        }

        public void ClearHistogram(String tag)
        {
            Histograms[tag].Items.Clear();
        }

        public void UpdateHistogram(String tag, ICollection<HistogramItem> items)
        {
            ClearHistogram(tag);
            AddHistogramItems(tag, items);
        }

        public PlotModel Model { get; private set; }
    }
}

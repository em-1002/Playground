using Arction.Wpf.Charting;
using Arction.Wpf.Charting.SeriesPolar;
using Arction.Wpf.Charting.SeriesXY;
using Playground.DataModels;

namespace Playground.Charting
{
    public class DrawPolar : DrawBase
    {        
        public override void Draw(IEnumerable<DataRecord> records)
        {
            //Plot the points... just use the index as the look.  IE record0 is look 0, record 270 is look 270...
            //   Use the R value as the amplitude/y value

            //InjectedChart.ViewPolar.PointLineSeries
            if (InjectedChart is null)
                return;
            List<PolarSeriesPoint> points = [];
            int i = 0;
            foreach (DataRecord record in records)
            {
                    points.Add(new PolarSeriesPoint(i++, record.R));
            }
            PointLineSeriesPolar pointSeries = new();
            pointSeries.AddPoints(points.ToArray(), false);
            InjectedChart.ViewPolar.PointLineSeries.Add(pointSeries);
        }
        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewPolar;
        }
    }
}

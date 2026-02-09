using Arction.Wpf.Charting;
using Arction.Wpf.Charting.SeriesPolar;
using Arction.Wpf.Charting.SeriesXY;
using Arction.Wpf.Charting.Titles;
using Playground.DataModels;
using System.Windows.Media;

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
            int points_idx = 0;
            double minR = double.MaxValue;
            double maxR = double.MinValue;
            foreach (DataRecord record in records)
            {
                double angle = points_idx++;
                double amplitude = record.R;
                points.Add(new PolarSeriesPoint(angle, amplitude));
                maxR = amplitude > maxR ? amplitude : maxR;
                minR = amplitude < minR ? amplitude : minR;
            }
            InjectedChart.ViewPolar.PointLineSeries.Clear();
            InjectedChart.ViewPolar.Sectors.Clear();
            if (points.Count > 0)
            {
                int num_sectors = 10;
                for (int i = 0; i < num_sectors; i++)
                {
                    Sector sector = new Sector();
                    sector.BeginAngle = 0 + i * (360 - 0) / num_sectors;
                    sector.EndAngle = 0 + (i + 1) * (360 - 0) / num_sectors;
                    List<PolarSeriesPoint> sector_points = points.Where(
                        p => p.Angle >= sector.BeginAngle 
                              && p.Angle < sector.EndAngle
                    ).ToList();
                    sector.Title.Text = $"Mean: {
                        Double.Round(
                            sector_points.Select(p => p.Amplitude).Average(), 2
                        )
                    }";
                    sector.Title.Visible = true;
                    sector.Title.Color = Colors.White;
                    sector.MinAmplitude = 0;
                    sector.MaxAmplitude = sector_points.Select(p => p.Amplitude)
                        .Max();
                    sector.AllowDragging = false;
                    sector.Behind = true;
                    InjectedChart.ViewPolar.Sectors.Add(sector);
                }
                PointLineSeriesPolar pointSeries = new();
                pointSeries.AddPoints(points.ToArray(), false);
                InjectedChart.ViewPolar.PointLineSeries.Add(pointSeries);
                double rMargin = (maxR - minR) * 0.1;
                rMargin = rMargin == 0 ? 1 : rMargin;
                InjectedChart.ViewPolar.Axes[0].MinAmplitude = minR - rMargin;
                InjectedChart.ViewPolar.Axes[0].MaxAmplitude = maxR + rMargin;
            }
        }
        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewPolar;
        }
    }
}

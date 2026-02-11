using Arction.Wpf.Charting;
using Arction.Wpf.Charting.Annotations;
using Arction.Wpf.Charting.EventMarkers;
using Arction.Wpf.Charting.SeriesPolar;
using Arction.Wpf.Charting.SeriesXY;
using Arction.Wpf.Charting.Titles;
using Playground.DataModels;
using System.Windows.Media;

namespace Playground.Charting
{
    public class DrawPolar : DrawBase
    {
        List<PointLineSeriesPolar> currentLineSeries = [];
        
        public override void Draw(IEnumerable<DataRecord> records, bool pointsVisible, Color lineColor)
        {
            //Plot the points... just use the index as the look.  IE record0 is look 0, record 270 is look 270...
            //   Use the R value as the amplitude/y value

            //InjectedChart.ViewPolar.PointLineSeries
            if (InjectedChart is null)
                return;
            if (!styleLoaded)
            {
                InjectedChart.ViewPolar.GraphBackground.Style = RectFillStyle.None;
                InjectedChart.ViewPolar.Axes[0].Title.Shadow.Style = TextShadowStyle.Off;
                InjectedChart.ViewPolar.Axes[0].Title.Color = Colors.Black;
                InjectedChart.ViewPolar.Axes[0].Units.Color = Colors.Black;
                InjectedChart.ViewPolar.Axes[0].MinorDivTickStyle.Color = Colors.Black;
                InjectedChart.ViewPolar.Axes[0].MajorDivTickStyle.Color = Colors.Black;
                InjectedChart.ViewPolar.Axes[0].AxisColor = Color.FromRgb(103, 140, 171);
                InjectedChart.ViewPolar.Axes[0].ScaleNibs.Color = Color.FromRgb(95, 177, 245);
                InjectedChart.ViewPolar.Axes[0].Title.Text = "'R' Values";
                InjectedChart.ViewPolar.AutoSizeMargins = false;
                InjectedChart.ViewPolar.Margins = new System.Windows.Thickness(50, 50, 50, 50);
                InjectedChart.ViewPolar.Axes[0].MajorGrid.LabelsColor = Colors.DarkSlateGray;
                InjectedChart.ViewPolar.Axes[0].MinorGrid.LabelsColor = Colors.DarkSlateGray;
                InjectedChart.ViewPolar.Axes[0].GridAngular.LabelsColor = Colors.DarkSlateGray;
                styleLoaded = true;
            }
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
            InjectedChart.ViewPolar.Markers.Clear();
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
                    sector.Title.Visible = false;
                    sector.Title.Color = Colors.Black;
                    sector.Fill.Color = Color.FromArgb(15, 0, 0, 0);
                    sector.BorderlineStyle.Color = Color.FromArgb(30, 0, 0, 0);
                    sector.BorderLocation = Sector.BorderStyle.Center;
                    sector.MinAmplitude = 0;
                    sector.MaxAmplitude = sector_points.Select(p => p.Amplitude)
                        .Max();
                    sector.AllowDragging = false;
                    sector.Behind = true;
                    InjectedChart.ViewPolar.Sectors.Add(sector);
                    PolarEventMarker meanMarker = new PolarEventMarker();
                    meanMarker.AngleValue = (sector.BeginAngle + sector.EndAngle) / 2;
                    meanMarker.Amplitude = sector_points.Select(p => p.Amplitude).Max() * 1.09;
                    meanMarker.Label.Text = $"Mean: {Double.Round(
                            sector_points.Select(p => p.Amplitude).Average(), 2
                        )}";
                    meanMarker.Label.Color = Colors.Black;
                    meanMarker.Symbol.Width = 0;
                    InjectedChart.ViewPolar.Markers.Add(meanMarker);
                }
                currentLineSeries.Clear();
                PointLineSeriesPolar data = new();
                currentLineSeries.Add(data);
                data.AddPoints(points.ToArray(), false);
                data.PointsVisible = pointsVisible;
                data.PointStyle.GradientFill = GradientFillPoint.Solid;
                data.PointStyle.Width /= 1.5;
                data.PointStyle.Height /= 1.5;
                data.PointStyle.Color1 = lineColor - Color.FromArgb(0, 110, 110, 110);
                data.LineStyle.Color = lineColor;
                InjectedChart.ViewPolar.PointLineSeries.Add(data);
                double rMargin = (maxR - minR) * 0.1;
                rMargin = rMargin == 0 ? 1 : rMargin;
                InjectedChart.ViewPolar.Axes[0].MinAmplitude = minR - rMargin;
                InjectedChart.ViewPolar.Axes[0].MaxAmplitude = maxR + rMargin;
            }
        }

        public override void SetPointsVisible(bool pointsVisible)
        {
            foreach (PointLineSeriesPolar data in currentLineSeries)
            {
                data.PointsVisible = pointsVisible;
            }
        }

        public override void SetLineColor(Color newColor)
        {
            foreach (PointLineSeriesPolar data in currentLineSeries)
            {
                data.LineStyle.Color = newColor;
                data.PointStyle.Color1 = newColor - Color.FromArgb(0, 110, 110, 110);
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

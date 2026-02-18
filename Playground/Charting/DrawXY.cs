using Arction.Wpf.Charting;
using Arction.Wpf.Charting.Axes;
using Arction.Wpf.Charting.SeriesXY;
using Arction.Wpf.Charting.Titles;
using Arction.Wpf.Charting.Views.ViewXY;
using Playground.DataModels;
using SharpDX.DXGI;
using System.Security.Cryptography;
using System.Windows.Media;

namespace Playground.Charting
{
    public class DrawXY : DrawBase
    {
        List<FreeformPointLineSeries> currentLineSeries = [];
        AxisX? xAxis;
        AxisY? yAxis;
        
        public override void Draw(IEnumerable<DataRecord> records, bool pointsVisible, Color lineColor)
        {
            //Given the records, draw a FreeFormPointLineSeries on the XY chart
            //InjectedChart.ViewXY.FreeformPointLineSeries

            if (InjectedChart is null)
                return;
            if (!styleLoaded)
            {
                xAxis = InjectedChart.ViewXY.XAxes[0];
                yAxis = InjectedChart.ViewXY.YAxes[0];
                ConfigureView(InjectedChart.ViewXY);
                ConfigureXAxis(xAxis);
                ConfigureYAxis(yAxis);
                styleLoaded = true;
            }

            List<SeriesPoint> points = [];
            int points_idx = 0;
            double maxX = double.MinValue;
            double minX = double.MaxValue;
            double maxY = double.MinValue;
            double minY = double.MaxValue;
            foreach (DataRecord record in records)
            {
                double x = points_idx++;
                double y = record.R;
                points.Add(new SeriesPoint(x, y));
                maxX = x > maxX ? x : maxX;
                minX = x < minX ? x : minX;
                maxY = y > maxY ? y : maxY;
                minY = y < minY ? y : minY;
            }

            InjectedChart.ViewXY.FreeformPointLineSeries.Clear();
            InjectedChart.ViewXY.Bands.Clear();
            if (points.Count > 0)
            {
                int num_bands = 10;
                for (int i = 0; i < num_bands; i++)
                {
                    Band band = new Band();
                    band.ValueBegin = minX + i * (maxX - minX) / num_bands;
                    band.ValueEnd = minX + (i+1) * (maxX - minX) / num_bands;
                    List<SeriesPoint> band_points = points
                        .Where(p => band.ValueBegin <= p.X && p.X < band.ValueEnd)
                        .ToList();
                    band.Title.Text = $"Mean: {
                        Double.Round(band_points.Select(p => p.Y).Average(), 2)
                    }";
                    ConfigureBand(band);
                    InjectedChart.ViewXY.Bands.Add(band);
                }
                currentLineSeries.Clear();
                FreeformPointLineSeries data = new();
                currentLineSeries.Add(data);
                data.AddPoints(points.ToArray(), false);
                InjectedChart.ViewXY.FreeformPointLineSeries.Add(data);

                data.PointsVisible = pointsVisible;
                data.LineStyle.Color = lineColor;
                data.PointStyle.GradientFill = GradientFillPoint.Solid;
                data.PointStyle.Width /= 1.5;
                data.PointStyle.Height /= 1.5;
                data.PointStyle.Color1 = lineColor 
                    - Color.FromArgb(0, 115, 115, 115);
                double xMargin = (maxX - minX) != 0 ? (maxX - minX) * 0.1 : 1;
                double yMargin = (maxY - minY) != 0 ? (maxY - minY) * 0.1 : 1;
                xAxis?.Minimum = minX - xMargin;
                xAxis?.Maximum = maxX + xMargin;
                yAxis?.Minimum = minY - xMargin;
                yAxis?.Maximum = maxY + xMargin;
            }
        }
        
        private static void ConfigureView(ViewXY view)
        {
            view.GraphBackground.Style = RectFillStyle.None;
            view.Border.Color = Color.FromRgb(103, 140, 171);
            view.AxisLayout.AutoAdjustMargins = false;
            view.Margins = new System.Windows.Thickness(50, 50, 50, 80);
        }

        private static void ConfigureXAxis(AxisX axis)
        {
			axis.Title.Shadow.Style = TextShadowStyle.Off;
			axis.Title.Color = Colors.Black;
			axis.LabelsColor = Colors.DarkSlateGray;
			axis.AxisColor = Color.FromRgb(103, 140, 171);
			axis.ScaleNibs.Color = Color.FromRgb(95, 177, 245);
			axis.Title.Text = "Indices";
        }

        private static void ConfigureYAxis(AxisY axis)
        {
            axis.Title.Shadow.Style = TextShadowStyle.Off;
            axis.Title.Color = Colors.Black;
            axis.LabelsColor = Colors.DarkSlateGray;
            axis.AxisColor = Color.FromRgb(103, 140, 171);
            axis.ScaleNibs.Color = Color.FromRgb(95, 177, 245);
            axis.Title.Text = "'R' Values";

        }

        private static void ConfigureBand(Band band)
        {
            band.Title.Visible = true;
            band.Title.Color = Colors.Black;
            band.Fill.GradientFill = GradientFill.Solid;
            band.Fill.Color = Color.FromArgb(15, 0, 0, 0);
            band.BorderColor = Color.FromArgb(30, 0, 0, 0);
            band.Behind = true;
            band.AllowMoveByUser = false;
            band.AllowResizeByUser = false;
        }

        public override void SetPointsVisible(bool pointsVisible)
        {
            foreach (FreeformPointLineSeries data in currentLineSeries)
            {
                data.PointsVisible = pointsVisible;
            }
        }

        public override void SetLineColor(Color newColor)
        {
            foreach (FreeformPointLineSeries data in currentLineSeries)
            {
                data.LineStyle.Color = newColor;
                data.PointStyle.Color1 = newColor - Color.FromArgb(0, 115, 115, 115);
            }
        }

        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewXY;
        }
    }
}

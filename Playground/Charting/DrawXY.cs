using Arction.Wpf.Charting;
using Arction.Wpf.Charting.SeriesXY;
using Arction.Wpf.Charting.Titles;
using Playground.DataModels;
using SharpDX.DXGI;
using System.Security.Cryptography;
using System.Windows.Media;

namespace Playground.Charting
{
    public class DrawXY : DrawBase
    {
        public override void Draw(IEnumerable<DataRecord> records)
        {
            //Given the records, draw a FreeFormPointLineSeries on the XY chart
            //InjectedChart.ViewXY.FreeformPointLineSeries

            if (InjectedChart is null)
                return;
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
                    List<SeriesPoint> band_points = points.Where(p => p.X >= band.ValueBegin && p.X < band.ValueEnd).ToList();
                    band.Title.Text = $"Mean: {Double.Round(band_points.Select(p => p.Y).Average(), 2)}";
                    band.Title.Visible = true;
                    band.Title.Color = Colors.White;
                    band.Behind = true;
                    band.AllowMoveByUser = false;
                    band.AllowResizeByUser = false;
                    InjectedChart.ViewXY.Bands.Add(band);
                }
                FreeformPointLineSeries data = new();
                data.AddPoints(points.ToArray(), false);
                InjectedChart.ViewXY.FreeformPointLineSeries.Add(data);
                double xMargin = (maxX - minX) * 0.1;
                double yMargin = (maxY - minY) * 0.1;
                InjectedChart.ViewXY.XAxes[0].Minimum = minX - xMargin;
                InjectedChart.ViewXY.XAxes[0].Maximum = maxX + xMargin;
                InjectedChart.ViewXY.YAxes[0].Minimum = minY - xMargin;
                InjectedChart.ViewXY.YAxes[0].Maximum = maxY + xMargin;
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

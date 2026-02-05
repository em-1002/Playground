using Arction.Wpf.Charting;
using Arction.Wpf.Charting.SeriesXY;
using Playground.DataModels;

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
            int i = 0;
            foreach (DataRecord record in records)
            {
                points.Add(new SeriesPoint(i++, record.R));

            }
            FreeformPointLineSeries data = new();
            data.AddPoints(points.ToArray(), false);
            InjectedChart.ViewXY.FreeformPointLineSeries.Add(data);
        }
        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewXY;
        }
    }
}

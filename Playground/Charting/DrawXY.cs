using Arction.Wpf.Charting;
using Playground.DataModels;

namespace Playground.Charting
{
    public class DrawXY : DrawBase
    {
        public override void Draw(IEnumerable<DataRecord> records)
        {
            //Given the records, draw a FreeFormPointLineSeries on the XY chart
            //InjectedChart.ViewXY.FreeformPointLineSeries
        }
        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewXY;
        }
    }
}

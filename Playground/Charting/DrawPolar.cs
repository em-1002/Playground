using Arction.Wpf.Charting;
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
        }
        public override void SetChartAppearance()
        {
            if (InjectedChart is null)
                return;
            InjectedChart.ActiveView = ActiveView.ViewPolar;
        }
    }
}

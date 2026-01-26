using Arction.Wpf.Charting;
using Playground.DataModels;

namespace Playground.Charting
{
    public abstract class DrawBase : IDraw
    {
        internal LightningChart? InjectedChart;


        public abstract void Draw(IEnumerable<DataRecord> records);
        public void InjectChart(LightningChart chart)
        {
            InjectedChart = chart;
            SetChartAppearance();
        }
        public abstract void SetChartAppearance();
    }
}

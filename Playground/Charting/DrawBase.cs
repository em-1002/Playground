using Arction.Wpf.Charting;
using Playground.DataModels;
using System.Windows.Media;

namespace Playground.Charting
{
    public abstract class DrawBase : IDraw
    {
        internal LightningChart? InjectedChart;


        public abstract void Draw(IEnumerable<DataRecord> records, bool pointsVisible, Color lineColor);
        public void InjectChart(LightningChart chart)
        {
            InjectedChart = chart;
            SetChartAppearance();
        }
        public abstract void SetChartAppearance();
        public abstract void SetPointsVisible(bool pointsVisible);
        public abstract void SetLineColor(Color newColor);
    }
}

using Arction.Wpf.Charting;
using System.Windows.Media;

namespace Playground.Charting
{
    public interface IDraw
    {
        public void InjectChart(LightningChart chart);
        public void Draw(IEnumerable<DataModels.DataRecord> records, bool pointsVisible, Color lineColor);
        public void SetPointsVisible(bool pointsVisible);
        public void SetLineColor(Color newColor);
    }
}

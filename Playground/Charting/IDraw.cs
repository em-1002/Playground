using Arction.Wpf.Charting;

namespace Playground.Charting
{
    public interface IDraw
    {
        public void InjectChart(LightningChart chart);
        public void Draw(IEnumerable<DataModels.DataRecord> records);
    }
}

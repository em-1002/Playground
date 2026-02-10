using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Playground.Charting
{
    public class ChartingViewModel(DatabaseBroker broker)
    {
        //How we obtain our data
        private readonly DatabaseBroker Broker = broker;
        public List<DataModels.Vector> Vectors { get; set; } = [];

        internal void Draw(DrawBase drawer, DataModels.Vector selectedVector, List<DataModels.Vector> vectors)
        {
            //We have the type of drawer we need (polar or xy) next:
            //- Create a ChartWindow.  It will auto-create the chart and inject the chart to the drawer
            //- 

            ChartWindow newWindow = new(drawer, Broker, selectedVector, vectors);
            newWindow.VectorsComboBox.SelectedIndex = vectors.IndexOf(selectedVector);
            newWindow.Show();
        }

    }
}

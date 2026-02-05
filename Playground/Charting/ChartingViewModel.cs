using System.Runtime.CompilerServices;

namespace Playground.Charting
{
    public class ChartingViewModel(DatabaseBroker broker)
    {
        //How we obtain our data
        private readonly DatabaseBroker Broker = broker;

        internal void Draw(DrawBase drawer, DataModels.Vector selectedVector)
        {
            //We have the type of drawer we need (polar or xy) next:
            //- Create a ChartWindow.  It will auto-create the chart and inject the chart to the drawer
            //- 

            ChartWindow newWindow = new(drawer, Broker, selectedVector);
            newWindow.DataContext = this;
            newWindow.Show();
        }
    }
}

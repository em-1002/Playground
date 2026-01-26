using System.Windows;
using Arction.Wpf.Charting;
using Playground.Charting;

namespace Playground
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public LightningChart Chart { get; private set; }

        private readonly DatabaseBroker Broker;
        private readonly IDraw Drawer;
        private DataModels.Vector SelectedVector;

        public ChartWindow(IDraw drawer, DatabaseBroker broker, DataModels.Vector selectedVector)
        {
            InitializeComponent();

            //Instantiate and add the chart to the form
            Chart = new() { ActiveView = ActiveView.ViewPolar };
            GridChart.Children.Add(Chart);

            //Inject the chart to our drawer
            Drawer = drawer;
            Drawer.InjectChart(Chart);

            //Capture the injected broker as this is how we get our data
            Broker = broker;

            //Capture our initial vector
            SelectedVector = selectedVector;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //1) Pull the data asychronously from the Broker based off the selected vector
            //2) Draw the data using the IDraw drawer            
        }
    }
}

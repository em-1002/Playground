using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging.Effects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
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
        public List<DataModels.Vector> Vectors { get; set; }
        public bool PointsVisible
        {
            get => _pointsVisible;
            set { _pointsVisible = value; Drawer.SetPointsVisible(value); }
        }
        private bool _pointsVisible = false;
        public Color LineColor
        {
            get => _lineColor;
            set { _lineColor = value; Drawer.SetLineColor(value); }
        }
        private Color _lineColor = Colors.Yellow;
        public Dictionary<String, Color> LineColors { get; set; } = new Dictionary<string, Color> 
        { 
            { "Red", Colors.Red },
            { "Blue", Colors.Blue },
            { "Yellow", Colors.Yellow },
            { "Green", Colors.Green },
        };
        public DataTable LineColorss { get; set; } = new DataTable();

        public ChartWindow(IDraw drawer, DatabaseBroker broker, DataModels.Vector selectedVector, List<DataModels.Vector> vectors)
        {
            InitializeComponent();
            DataContext = this;

            Vectors = vectors;

            //Instantiate and add the chart to the form
            Chart = new() { ActiveView = ActiveView.ViewPolar };
            Chart.Title.Text = "Random Values";
            Chart.Title.Shadow.Style = TextShadowStyle.Off;
            Chart.Title.Color = Colors.Black;
            Chart.ChartBackground.Color = (Color) ColorConverter.ConvertFromString("#00000000");
            Chart.ChartBackground.GradientFill = GradientFill.Solid;
            GridChart.Children.Add(Chart);

            //Inject the chart to our drawer
            Drawer = drawer;
            Drawer.InjectChart(Chart);

            //Capture the injected broker as this is how we get our data
            Broker = broker;

            //Capture our initial vector
            SelectedVector = selectedVector;

            LineColorss.Columns.Add("Name", typeof(String));
            LineColorss.Columns.Add("Color", typeof(Color));
            LineColorss.Rows.Add("Red", Colors.Red);
            LineColorss.Rows.Add("Blue", Colors.Blue);
            LineColorss.Rows.Add("Yellow", Colors.Yellow);
            LineColorss.Rows.Add("Green", Colors.Green);
            //LineColors.

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //1) Pull the data asychronously from the Broker based off the selected vector
            List<DataModels.DataRecord> results = await Broker.GetDataRecordsAsync(SelectedVector.VectorId);
            //2) Draw the data using the IDraw drawer            
            Drawer.Draw(results, PointsVisible, LineColor);
        }

        private async void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedVector = (DataModels.Vector) VectorsComboBox.SelectedItem;
            List<DataModels.DataRecord> results = await Broker.GetDataRecordsAsync(SelectedVector.VectorId);
            Drawer.Draw(results, PointsVisible, LineColor);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (VectorsComboBox.SelectedIndex == 0)
                {
                    VectorsComboBox.SelectedIndex = VectorsComboBox.Items.Count - 1;
                }
                else
                {
                    VectorsComboBox.SelectedIndex -= 1;
                }
            }
            if (e.Key == Key.Right)
            {
                if (VectorsComboBox.SelectedIndex == VectorsComboBox.Items.Count-1)
                {
                    VectorsComboBox.SelectedIndex = 0;
                }
                else
                {
                    VectorsComboBox.SelectedIndex += 1;
                }
            }
        }

    }
}

using System.Windows;
using System.Windows.Controls;
using Playground.Charting;

namespace Playground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel mvm)
                return;
            await mvm.LoadVectors();
        }

        private void BtnShowChart_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Draw_Clicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel mvm
                || LstVectors.SelectedItem is not DataModels.Vector selectedVector 
                                || sender is not Button bt
                                || bt.Tag is not Type t)
                return;
            //create instance of the class we need based off the button.Tag we declared in the xaml
            var instance = Activator.CreateInstance(t);
            if (instance is DrawBase drawer)
                mvm.Charting.Draw(drawer, selectedVector, mvm.Data.Vectors);
        }
    }
}
namespace Playground;

public class MainViewModel
{
    public MainViewModel()
    {
        Charting = new(Broker);
    }

    /// <summary>
    /// Store some data here for use
    /// </summary>
    public LoadedDataViewModel Data { get; } = new();

    /// <summary>
    /// This broker is the interface into the database
    /// </summary>
    public DatabaseBroker Broker { get; } = new();

    public Charting.ChartingViewModel Charting { get; }


    internal async Task LoadVectors()
    {
        Data.SetVectors((await Broker.GetVectorsAsync()));
    }
}

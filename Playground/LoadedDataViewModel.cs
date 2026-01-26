namespace Playground;

public class LoadedDataViewModel : PropChange
{
    public List<DataModels.Vector> Vectors { get => _vectors; private set { _vectors = value; } }
    private List<DataModels.Vector> _vectors = [];
    internal void SetVectors(List<DataModels.Vector> incoming) { _vectors = incoming; OnPropertyChanged(nameof(Vectors)); }
}

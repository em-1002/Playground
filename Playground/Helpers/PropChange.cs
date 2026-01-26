using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Playground;

/// <summary>
/// Class inherits INotify to tell the UI to update their bound properties when that property changes
/// </summary>
public class PropChange : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
using System.ComponentModel;

namespace SyncMe.ViewModels;

public abstract class BaseEventViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void ChangeProperty<T>(ref T internalValue, T newValue, string propertyName)
    {
        if (!EqualityComparer<T>.Default.Equals(internalValue, newValue))
        {
            internalValue = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}

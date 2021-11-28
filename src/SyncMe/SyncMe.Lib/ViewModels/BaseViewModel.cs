using System.ComponentModel;

namespace SyncMe.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool ChangeProperty<T>(ref T internalValue, T newValue, string propertyName)
    {
        if (!EqualityComparer<T>.Default.Equals(internalValue, newValue))
        {
            internalValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        return false;
    }
}

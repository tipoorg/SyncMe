using System.ComponentModel;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace SyncMe.Models
{
    public class NamespaceModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private bool _isButtonsVisible;
        public bool IsButtonsVisible
        {
            get { return _isButtonsVisible; }
            set 
            {
                _isButtonsVisible = value;
                OnPropertyChanged(nameof(IsButtonsVisible));
            }
        }

        public bool IsActive { get; set; }

        public ICommand TomorrowClick { private set; get; }
        public static Subject<NamespaceModel> TomorrowClicked { private set; get; } = new Subject<NamespaceModel>();

        public ICommand RestoreClick { private set; get; }
        public static Subject<NamespaceModel> RestoreClicked { private set; get; } = new Subject<NamespaceModel>();

        public NamespaceModel(int id, string name)
        {
            Name = name;
            Id = id;
            TomorrowClick = new Command(() => TomorrowClicked.OnNext(this));
            RestoreClick = new Command(() => RestoreClicked.OnNext(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}

using System.ComponentModel;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace SyncMe.Models
{
    public class NamespaceModel : INotifyPropertyChanged
    {
        public string Name { get; private set; }

        public string FullName { get; private set; }

        private bool _isButtonsVisible;
        public bool IsButtonsVisible
        {
            get { return _isButtonsVisible; }
            set { _isButtonsVisible = value; OnPropertyChanged(nameof(IsButtonsVisible)); }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set 
            { 
                _isExpanded = value; 
                OnPropertyChanged(nameof(IsExpanded));
                OnPropertyChanged(nameof(ArrowRotation));
            }
        }

        public Thickness Margin
        {
            get
            {
                var dots = FullName.Count(c => c == '.') + 1;
                return new Thickness(dots * 8, 5, 8, 5);
            }
        }

        public string ArrowRotation
        {
            get { return IsExpanded ? "180"  : "0"; }
        }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }

        public ICommand TomorrowClick { private set; get; }
        public static Subject<NamespaceModel> TomorrowClicked { private set; get; } = new Subject<NamespaceModel>();

        public ICommand RestoreClick { private set; get; }
        public static Subject<NamespaceModel> RestoreClicked { private set; get; } = new Subject<NamespaceModel>();

        public ICommand ExpandClick { private set; get; }
        public static Subject<NamespaceModel> ExpandClicked { private set; get; } = new Subject<NamespaceModel>();

        public NamespaceModel(string fullName)
        {
            FullName = fullName;
            Name = FullName;
            IsVisible = !fullName.Contains(".");

            TomorrowClick = new Command(() => TomorrowClicked.OnNext(this));
            RestoreClick = new Command(() => RestoreClicked.OnNext(this));
            ExpandClick = new Command(() => ExpandClicked.OnNext(this));
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

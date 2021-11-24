using System.ComponentModel;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace SyncMe.Models
{
    public class NamespaceModel : INotifyPropertyChanged
    {
        public string Name { get; private set; }

        public string FullName { get; private set; }


        private bool _isSuspendButtonsVisible;
        public bool IsSuspendButtonsVisible
        {
            get { return _isSuspendButtonsVisible; }
            set 
            {
                if (value == _isSuspendButtonsVisible) return;
                _isSuspendButtonsVisible = value; 
                OnPropertyChanged(nameof(IsSuspendButtonsVisible)); 
            }
        }

        private bool _isRestoreButtonsVisible;
        public bool IsRestoreButtonsVisible
        {
            get { return _isRestoreButtonsVisible; }
            set 
            {
                if (value == _isRestoreButtonsVisible) return;
                _isRestoreButtonsVisible = value; 
                OnPropertyChanged(nameof(IsRestoreButtonsVisible)); 
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set 
            {
                if(value == _isExpanded) return;
                _isExpanded = value;
                
                OnPropertyChanged(nameof(IsExpanded));
                OnPropertyChanged(nameof(ArrowRotation));
            }
        }

        private bool _hasChildren;
        public bool HasChildren
        {
            get { return _hasChildren; }
            set
            {
                if(value == _hasChildren) return;
                _hasChildren = value;
                OnPropertyChanged(nameof(HasChildren));
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

        public string BorderColor { get => IsActive ? "#39c2d7" : "#8e244d"; }

        public bool IsVisible { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set 
            {
                if (_isActive == value) return;
                _isActive = value;

                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(BorderColor)); 
            }
        }

        public ICommand TomorrowClick { private set; get; }
        public static Subject<NamespaceModel> TomorrowClicked { private set; get; } = new Subject<NamespaceModel>();

        public ICommand RestoreClick { private set; get; }
        public static Subject<NamespaceModel> RestoreClicked { private set; get; } = new Subject<NamespaceModel>();

        public ICommand ExpandClick { private set; get; }
        public static Subject<NamespaceModel> ExpandClicked { private set; get; } = new Subject<NamespaceModel>();

        public NamespaceModel(string fullName, bool isActive, bool hasChildren)
        {
            FullName = fullName;
            Name = FullName.Split('.').Last();
            _isActive = isActive;
            _hasChildren = hasChildren;

            TomorrowClick = new Command(() => TomorrowClicked.OnNext(this));
            RestoreClick = new Command(() => RestoreClicked.OnNext(this));
            ExpandClick = new Command(() => ExpandClicked.OnNext(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

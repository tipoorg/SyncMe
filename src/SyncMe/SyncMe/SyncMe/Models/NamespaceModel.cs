using System.Reactive.Subjects;
using System.Windows.Input;

namespace SyncMe.Models
{
    public class NamespaceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsVisible { get; set; }

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
    }
}

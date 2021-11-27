using System.Collections.ObjectModel;
using SyncMe.Functional;
using SyncMe.Lib.Extensions;
using SyncMe.Lib.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NamespaceManagmentPage : ContentPage
{
    public ObservableCollection<NamespaceViewModel> Namespaces { get; set; }

    private readonly ISyncNamespaceService _namespaceService;

    public NamespaceManagmentPage(ISyncNamespaceService namespaceRepository)
    {
        InitializeComponent();
        _namespaceService = namespaceRepository;

        Namespaces = new ObservableCollection<NamespaceViewModel>(_namespaceService
            .GetFirstLevel()
            .Select(s => new NamespaceViewModel(s.FullName, s.IsActive, s.HasChilde)));

        NamespaceViewModel.TomorrowClicked.Subscribe(MoveToSuspended);
        NamespaceViewModel.RestoreClicked.Subscribe(MoveToActive);
        NamespaceViewModel.ExpandClicked.Subscribe(ProcessExpanding);
        NamespaceViewModel.NewItemClicked.Subscribe(AddNewNamespace);
        NamespaceViewModel.RemoveClicked.Subscribe(RemoveItemWithChildrens);
        BindingContext = this;
    }

    private void activeNamespaces_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var tappedItem = e.Item as NamespaceViewModel;

        if (tappedItem.IsActive)
        {
            var prevValue = tappedItem.IsSuspendButtonsVisible;
            Namespaces.CollapseAllButtons();
            tappedItem.IsSuspendButtonsVisible = !prevValue;
        }
        else
        {
            var prevValue = tappedItem.IsRestoreButtonsVisible;
            Namespaces.CollapseAllButtons();
            tappedItem.IsRestoreButtonsVisible = !prevValue;
        }
        Namespaces.RemoveAndInsertItem(tappedItem);
    }

    private void MoveToSuspended(NamespaceViewModel item)
    {
        item.IsSuspendButtonsVisible = false;
        SuspendAllChildren(item);
    }

    private void MoveToActive(NamespaceViewModel item)
    {
        item.IsRestoreButtonsVisible = false;
        RestoreAllChildren(item);
    }

    private void ProcessExpanding(NamespaceViewModel item)
    {
        if (!item.IsExpanded)
        {
            AddFirstLevelChildrenn(item.FullName);
        }
        else
        {
            Namespaces.RemoveAllChildren(item.FullName);
        }
        item.IsExpanded = !item.IsExpanded;
    }

    private void SuspendAllChildren(NamespaceViewModel item)
    {
        if (!_namespaceService.UpdateStatusWithChildrens(item.FullName, false, DateTime.Now.AddDays(1)))
            return;

        item.IsActive = false;
        Namespaces.ApplyToAllChildren(item.FullName, s => s.IsActive = false);
    }

    private void RestoreAllChildren(NamespaceViewModel item)
    {
        if (!_namespaceService.UpdateStatusWithChildrens(item.FullName, true))
            return;

        item.IsActive = true;
        Namespaces.ApplyToAllChildren(item.FullName, s => s.IsActive = true);
    }

    private void AddFirstLevelChildrenn(string fullName)
    {
        var children = _namespaceService.GetFirstChildren(fullName);
        var parentIndex = Namespaces.ToList().FindIndex(s => s.FullName == fullName);
        Namespaces.InsertRange(parentIndex, children.ToList());
    }

    private async void AddNewNamespace(NamespaceViewModel parent)
    {
        string newName = await DisplayPromptAsync("Enter new namespace name", $"{parent.FullName}.", "Add", "Cancel", null, 10);
        if (string.IsNullOrWhiteSpace(newName))
        {
            return;
        }

        var newFullName = $"{parent.FullName}.{newName}";
        _namespaceService.Add(newFullName);

        if (!parent.IsExpanded)
        {
            AddFirstLevelChildrenn(parent.FullName);
            parent.IsExpanded = true;
        }
        else
        {
            Namespaces.Insert(Namespaces.IndexOf(parent) + 1, new NamespaceViewModel(
                fullName: newFullName,
                isActive: parent.IsActive,
                hasChildren: false
                ));
        }
        parent.HasChildren = true;
        parent.IsExpanded = true;
    }

    private async void RemoveItemWithChildrens(NamespaceViewModel item)
    {
        var result = await DisplayAlert("Remove namespace", $"Delete {item.FullName} and all nested Namespaces?", "Yes", "No");
        if (!result) return;

        _namespaceService.RemoveWithChildren(item.FullName);
        Namespaces.RemoveAllChildren(item.FullName);
        Namespaces.Remove(item);

        ProcessParentHasChildren(item);
    }

    public void ProcessParentHasChildren(NamespaceViewModel item)
    {
        if (!item.FullName.Contains('.'))
            return;

        var parentName = item.FullName.Split('.').Where(n => n != item.Name).FeedTo(s => string.Join(".", s));

        if (!_namespaceService.HasChildren(parentName))
        {
            var parentItem = Namespaces.FirstOrDefault(s => s.FullName == parentName).HasChildren = false;
        }
    }
}

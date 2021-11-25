using System.Collections.ObjectModel;
using SyncMe.Models;
using SyncMe.Services;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NamespaceManagmentPage : ContentPage
{
    public ObservableCollection<NamespaceModel> Namespaces { get; set; }

    private readonly ISyncNamespaceService _namespaceService;

    public NamespaceManagmentPage(ISyncNamespaceService namespaceRepository)
    {
        InitializeComponent();
        _namespaceService = namespaceRepository;

        Namespaces = new ObservableCollection<NamespaceModel>(_namespaceService.
            GetFirstLevel()
            .Select(s => new NamespaceModel(s.FullName, s.IsActive, s.HasChilde)));

        NamespaceModel.TomorrowClicked.Subscribe(MoveToSuspended);
        NamespaceModel.RestoreClicked.Subscribe(MoveToActive);
        NamespaceModel.ExpandClicked.Subscribe(ProcessExpanding);
        NamespaceModel.NewItemClicked.Subscribe(AddNewNamespace);
        BindingContext = this;
    }

    private void activeNamespaces_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var tappedItem = e.Item as NamespaceModel;

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

    private void MoveToSuspended(NamespaceModel item)
    {
        item.IsSuspendButtonsVisible = false;
        SuspendAllChildren(item);
    }

    private void MoveToActive(NamespaceModel item)
    {
        item.IsRestoreButtonsVisible = false;
        RestoreAllChildren(item);
    }

    private void ProcessExpanding(NamespaceModel item)
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

    private void SuspendAllChildren(NamespaceModel item)
    {
        if(!_namespaceService.UpdateStatusWithChildrens(item.FullName, false, DateTime.Now.AddDays(1)))
            return;

        item.IsActive = false;
        Namespaces.ApplyToAllChildren(item.FullName, s => s.IsActive = false);
    }

    private void RestoreAllChildren(NamespaceModel item)
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

    private async void AddNewNamespace(NamespaceModel parent)
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
            Namespaces.Insert(Namespaces.IndexOf(parent) + 1, new NamespaceModel(
                fullName: newFullName,
                isActive: parent.IsActive,
                hasChildren: false
                ));
        }
        parent.HasChildren = true;
        parent.IsExpanded = true;
    }
}

public static class NamespaceExtensions
{
    public static void RemoveAllChildren(this ObservableCollection<NamespaceModel> items, string fullName)
    {
        var itemToRemove = items.FirstOrDefault(n => n.FullName.Contains($"{fullName}."));

        if (itemToRemove is null) return;

        items.Remove(itemToRemove);
        RemoveAllChildren(items, fullName);
    }

    public static void ApplyToAllChildren(this ObservableCollection<NamespaceModel> items, string fullName, Action<NamespaceModel> action)
    {
        items.Where(n => n.FullName.Contains($"{fullName}.")).ToList().ForEach(action);
    }

    public static void CollapseAllButtons(this ObservableCollection<NamespaceModel> items)
    {
        foreach (var item in items)
        {
            item.IsSuspendButtonsVisible = false;
            item.IsRestoreButtonsVisible = false;
        }
    }

    public static void InsertRange(this ObservableCollection<NamespaceModel> items, int index, IList<(string FullName, bool IsActive, bool HasChildren)> children)
    {
        for (int i = 0; i < children.Count; i++)
        {
            items.Insert(index + i + 1, new NamespaceModel(children[i].FullName, children[i].IsActive, children[i].HasChildren));
        }
    }

    public static void RemoveAndInsertItem(this ObservableCollection<NamespaceModel> items, NamespaceModel item)
    {
        var index = items.IndexOf(item);
        items.RemoveAt(index);
        items.Insert(index, item);
    }
}

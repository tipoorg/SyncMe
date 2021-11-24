using System.Collections.ObjectModel;
using SyncMe.Models;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NamespaceManagmentPage : ContentPage
{
    private string[] active = new string[] {"Work1" ,"Work1.Team1", "Work1.Team1.Project1", "Work1.Team1.Project1", "Work2", "Work2.Team1", "Work1.Team2", "Work1.Team3"};
    public ObservableCollection<NamespaceModel> Namespaces { get; set; }

    public NamespaceManagmentPage()
    {
        InitializeComponent();

        Namespaces = new ObservableCollection<NamespaceModel>(active.GetFirstLevelSpaces());

        NamespaceModel.TomorrowClicked.Subscribe(MoveToSuspended);
        NamespaceModel.RestoreClicked.Subscribe(MoveToActive);
        NamespaceModel.ExpandClicked.Subscribe(ProcessExpanding);
        BindingContext = this;
    }

    private void activeNamespaces_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var tapped = e.Item as NamespaceModel;

        if (tapped.IsActive)
        {
            var prevValue = tapped.IsSuspendButtonsVisible;
            foreach (var item in Namespaces)
            {
                if (item.IsSuspendButtonsVisible)
                {
                    item.IsSuspendButtonsVisible = false;
                    item.IsRestoreButtonsVisible = false;
                }
            }
            tapped.IsSuspendButtonsVisible = !prevValue;
        }
        else
        {
            var prevValue = tapped.IsRestoreButtonsVisible;
            foreach (var item in Namespaces)
            {
                if (item.IsSuspendButtonsVisible)
                {
                    item.IsSuspendButtonsVisible = false;
                    item.IsRestoreButtonsVisible = false;
                }
            }
            tapped.IsRestoreButtonsVisible = !prevValue;
        }
    }

    private void MoveToSuspended(NamespaceModel item)
    {
        item.IsSuspendButtonsVisible = false;
        item.IsActive = false;
        SuspendAllChildren(item);
    }

    private void MoveToActive(NamespaceModel item)
    {
        item.IsRestoreButtonsVisible = false;
        item.IsActive = true;
        RestoreAllChildren(item);
    }

    private void ProcessExpanding(NamespaceModel item)
    {
        if (!item.IsExpanded)
        {
            Namespaces.AddFirstLevelChildren(item.FullName, active);
            item.IsExpanded = true;
        }
        else
        {
            Namespaces.RemoveAllChildren(item.FullName);
            item.IsExpanded = false;
        }
    }

    private void SuspendAllChildren(NamespaceModel item)
    {
        //suspend in base and local state
        Namespaces.ApplyToAllChildren(item.FullName, s => s.IsActive = false);
    }

    private void RestoreAllChildren(NamespaceModel item)
    {
        //restore in base and local state
        Namespaces.ApplyToAllChildren(item.FullName, s => s.IsActive = true);
    }
}

public static class NamespaceHelper
{
    public static IEnumerable<NamespaceModel> GetFirstLevelSpaces(this string[] namespaces) => 
        namespaces
        .Where(s => !s.Contains('.'))
        .Select(s => new NamespaceModel(s, true, s.HasChildren(namespaces)));
    
    public static void AddFirstLevelChildren(this ObservableCollection<NamespaceModel> items, string fullName, string[] namespaces)
    {
        var childrenNames = namespaces
            .Where( n => n.Count(s => s == '.') == fullName.Count(s => s == '.') + 1 && n.StartsWith($"{fullName}."))
            .ToList();
        var parentIndex = items.ToList().FindIndex(s => s.FullName == fullName);

        for (int i = 0; i < childrenNames.Count() ; i++)
        {
            items.Insert(parentIndex + i + 1, new NamespaceModel(childrenNames[i], true, childrenNames[i].HasChildren(namespaces)));
        }
    }

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

    public static bool HasChildren(this string fullName, string[] items)
    {
        return items.Any(n => n.Contains($"{fullName}."));
    }
}

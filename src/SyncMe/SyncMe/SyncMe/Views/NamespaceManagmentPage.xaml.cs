using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SyncMe.Models;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class NamespaceManagmentPage : ContentPage
{
    private string[] active = new string[] { "Work1", "Work1.Team1", "Work1.Team1.Project1", "Work1.Team1.Project1", "Work2", "Work2.Team1", "Work1.Team2", "Work1.Team3"};
    private string[] suspended = new string[] { "1Work1", "1Work1.Team1", "1Work1.Team1.Project1", "1Work1.Team1.Project1", "1Work2", "1Work2.Team1", "1Work1.Team2", "1Work1.Team3" };
    public ObservableCollection<NamespaceModel> ActiveNamespaces { get; set; }
    public ObservableCollection<NamespaceModel> SuspendedNamespaces { get; set; }

    public NamespaceManagmentPage()
    {
        InitializeComponent();

        ActiveNamespaces = new ObservableCollection<NamespaceModel>(active.GetFirstLevelSpaces());
        SuspendedNamespaces = new ObservableCollection<NamespaceModel>(suspended.GetFirstLevelSpaces());

        NamespaceModel.TomorrowClicked.Subscribe(MoveToSuspended);
        NamespaceModel.RestoreClicked.Subscribe(MoveToActive);
        NamespaceModel.ExpandClicked.Subscribe(ProcessExpanding);
        BindingContext = this;
    }

    private void activeNamespaces_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ProccessTapping(e, ActiveNamespaces);
    }

    private void suspendedNamespaces_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ProccessTapping(e, SuspendedNamespaces);
    }

    private void ProccessTapping(ItemTappedEventArgs e, ObservableCollection<NamespaceModel> items)
    {
        var tapped = e.Item as NamespaceModel;
        tapped.IsButtonsVisible = !tapped.IsButtonsVisible;
        var index = items.IndexOf(tapped);
        items.Remove(tapped);
        foreach (var item in items)
        {
            if (item.IsButtonsVisible)
            {
                item.IsButtonsVisible = false;
            }
        }
        items.Insert(index, tapped);
    }

    private void MoveToSuspended(NamespaceModel item)
    {
        ActiveNamespaces.Remove(item);
        item.IsButtonsVisible = false;
        item.IsActive = false;
        SuspendedNamespaces.Add(item);
    }

    private void MoveToActive(NamespaceModel item)
    {
        SuspendedNamespaces.Remove(item);
        item.IsButtonsVisible = false;
        item.IsActive = true;
        ActiveNamespaces.Add(item);
    }

    private void ProcessExpanding(NamespaceModel item)
    {
        var collection = ActiveNamespaces.Contains(item) ? ActiveNamespaces : SuspendedNamespaces;
        if (!item.IsExpanded)
        {
            collection.AddFirstLevelChildren(item.FullName, active.Concat(suspended).ToArray());
            item.IsExpanded = true;
        }
        else
        {
            collection.RemoveAllChildren(item.FullName);
            item.IsExpanded = false;
        }
    }
}


public static class NamespaceHelper
{
    public static IEnumerable<NamespaceModel> GetFirstLevelSpaces(this string[] namespaces) => 
        namespaces
        .Where(s => !s.Contains('.'))
        .Select(s => new NamespaceModel(s));
    
    public static void AddFirstLevelChildren(this ObservableCollection<NamespaceModel> items, string fullName, string[] namespaces)
    {
        var childrenNames = namespaces
            .Where( n => n.Count(s => s == '.') == fullName.Count(s => s == '.') + 1 && n.StartsWith($"{fullName}."))
            .ToList();
        var parentIndex = items.ToList().FindIndex(s => s.FullName == fullName);

        for (int i = 0; i < childrenNames.Count() ; i++)
        {
            items.Insert(parentIndex + i + 1, new NamespaceModel(childrenNames[i]));
        }
    }

    public static void RemoveAllChildren(this ObservableCollection<NamespaceModel> items, string fullName)
    {
        var itemToRemove = items.FirstOrDefault(n => n.FullName.Contains($"{fullName}."));

        if (itemToRemove is null) return;

        items.Remove(itemToRemove);
        RemoveAllChildren(items, fullName);
    }
}

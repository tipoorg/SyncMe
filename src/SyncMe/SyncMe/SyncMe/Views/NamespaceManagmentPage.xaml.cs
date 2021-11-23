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
    private string[] sss = new string[] { "Namespace1", "Namespace2", "Namespace3", "Namespace4", "Namespace5", "Namespace6", "Namespace7", "Namespace8" };
    public ObservableCollection<NamespaceModel> ActiveNamespaces { get; set; }
    public ObservableCollection<NamespaceModel> SuspendedNamespaces { get; set; }

    public NamespaceManagmentPage()
    {
        InitializeComponent();

        ActiveNamespaces = new ObservableCollection<NamespaceModel>(sss.Select((s, i) => new NamespaceModel(i, s)).ToList());
        SuspendedNamespaces = new ObservableCollection<NamespaceModel>(sss.Select((s,i) => new NamespaceModel(i,s)).ToList());

        NamespaceModel.TomorrowClicked.Subscribe(MoveToSuspended);
        NamespaceModel.RestoreClicked.Subscribe(MoveToActive);
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
        var tappedNamespace = e.Item as NamespaceModel;
        tappedNamespace.IsVisible = !tappedNamespace.IsVisible;
        var index = items.IndexOf(tappedNamespace);
        items.RemoveAt(index);
        items.Insert(index, tappedNamespace);
    }

    private void MoveToSuspended(NamespaceModel item)
    {
        ActiveNamespaces.Remove(item);
        item.IsVisible = false;
        item.IsActive = false;
        SuspendedNamespaces.Add(item);
    }
    private void MoveToActive(NamespaceModel item)
    {
        SuspendedNamespaces.Remove(item);
        item.IsVisible = false;
        item.IsActive = true;
        ActiveNamespaces.Add(item);
    }
}

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
        var tapped = e.Item as NamespaceModel;
        tapped.IsButtonsVisible = !tapped.IsButtonsVisible;
        foreach (var item in items.Where(s => !s.Equals(tapped)))
        {
            if (item.IsButtonsVisible)
            {
                item.IsButtonsVisible = false;
            }
        }
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
}

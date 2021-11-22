using SyncMe.Models;
using SyncMe.ViewModels;

namespace SyncMe.Views;

public partial class NewItemPage : ContentPage
{
    public Item Item { get; set; }

    public NewItemPage()
    {
        InitializeComponent();
        BindingContext = new NewItemViewModel();
    }
}

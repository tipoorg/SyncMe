using SyncMe.ViewModels;

namespace SyncMe.Views;

public partial class ItemDetailPage : ContentPage
{
    public ItemDetailPage()
    {
        InitializeComponent();
        BindingContext = new ItemDetailViewModel();
    }
}

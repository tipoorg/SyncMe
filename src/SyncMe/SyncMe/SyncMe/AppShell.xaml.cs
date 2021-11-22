using SyncMe.Views;

namespace SyncMe;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
    }
}

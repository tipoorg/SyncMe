using System;
using System.Collections.Generic;
using SyncMe.ViewModels;
using SyncMe.Views;
using Xamarin.Forms;

namespace SyncMe
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}

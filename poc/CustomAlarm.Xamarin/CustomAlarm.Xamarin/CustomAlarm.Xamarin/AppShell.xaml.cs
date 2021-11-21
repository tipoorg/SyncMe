using System;
using System.Collections.Generic;
using CustomAlarm.Xamarin.ViewModels;
using CustomAlarm.Xamarin.Views;
using Xamarin.Forms;

namespace CustomAlarm.Xamarin
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

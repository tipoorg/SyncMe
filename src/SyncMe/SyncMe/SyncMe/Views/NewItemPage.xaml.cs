using System;
using System.Collections.Generic;
using System.ComponentModel;
using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
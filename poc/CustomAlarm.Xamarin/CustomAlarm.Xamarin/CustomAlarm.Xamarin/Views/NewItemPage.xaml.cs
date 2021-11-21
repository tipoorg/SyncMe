using System;
using System.Collections.Generic;
using System.ComponentModel;
using CustomAlarm.Xamarin.Models;
using CustomAlarm.Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomAlarm.Xamarin.Views
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
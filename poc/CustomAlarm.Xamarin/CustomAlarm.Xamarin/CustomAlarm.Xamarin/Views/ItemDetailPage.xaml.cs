using System.ComponentModel;
using CustomAlarm.Xamarin.ViewModels;
using Xamarin.Forms;

namespace CustomAlarm.Xamarin.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
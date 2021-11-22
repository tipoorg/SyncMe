using System.ComponentModel;
using SyncMe.ViewModels;
using Xamarin.Forms;

namespace SyncMe.Views
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
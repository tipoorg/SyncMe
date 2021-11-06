using System;
using Microsoft.Maui.Controls;

namespace CustomAlarm.MAUI
{
    public partial class MainPage : ContentPage
    {
        public static event EventHandler OnSetAlarmClickedEvent;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnSetAlarmClicked(object sender, EventArgs e)
        {
            OnSetAlarmClickedEvent(sender, e);
        }
    }
}

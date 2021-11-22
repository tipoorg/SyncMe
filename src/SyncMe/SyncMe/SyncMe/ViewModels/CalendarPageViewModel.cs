using SyncMe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels
{
    internal class CalendarPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        EventCollection events = new EventCollection()
        {
            [DateTime.Now] = new List<EventModel>
                {
                    new EventModel() { Description = "Happy day", Name = "Birthday"},
                    new EventModel() { Description = "Very happy day", Name = "New year"}
                },
            [DateTime.Now.AddDays(5)] = new List<EventModel>
                {
                    new EventModel { Name = "Win", Description = "Our win day" },
                    new EventModel { Name = "Winter", Description = "Very cold time day" }
                },
            // 3 days ago
            [DateTime.Now.AddDays(-4)] = new List<EventModel>
                {
                    new EventModel { Name = "Key", Description = "Very amazing key day" }
                },
        };

        public EventCollection Events {
            get => events;
            set => events = value;
        
        }
    };
}


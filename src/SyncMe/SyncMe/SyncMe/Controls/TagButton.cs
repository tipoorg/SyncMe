using SyncMe.Models;

namespace SyncMe.Controls;

public class TagButton : Button
{
    public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag", typeof(SyncReminder), typeof(TagButton), SyncReminder.AtEventTime, BindingMode.OneTime);

    public SyncReminder Tag
    {
        set
        {
            SetValue(TagProperty, value);
        }
        get
        {
            return (SyncReminder)GetValue(TagProperty);
        }
    }
}

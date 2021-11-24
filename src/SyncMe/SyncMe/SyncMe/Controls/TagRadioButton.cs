using SyncMe.Models;

namespace SyncMe.Controls;

public class TagRadioButton : RadioButton
{
    public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag", typeof(SyncRepeat), typeof(TagRadioButton), SyncRepeat.None, BindingMode.OneTime);

    public SyncRepeat Tag
    {
        set
        {
            SetValue(TagProperty, value);
        }
        get
        {
            return (SyncRepeat)GetValue(TagProperty);
        }
    }
}

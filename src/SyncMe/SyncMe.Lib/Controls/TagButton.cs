namespace SyncMe.Controls;

public class TagButton<T> : Button
{
    public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag", typeof(T), typeof(TagButton<T>), default(T), BindingMode.OneTime);

    public T Tag
    {
        set
        {
            SetValue(TagProperty, value);
        }
        get
        {
            return (T)GetValue(TagProperty);
        }
    }
}

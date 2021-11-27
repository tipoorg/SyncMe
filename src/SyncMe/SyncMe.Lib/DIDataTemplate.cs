using Xamarin.Forms.Xaml;

namespace SyncMe;

public sealed class DIDataTemplate : IMarkupExtension<DataTemplate>
{
    public static IServiceProvider AppServiceProvider { get; set; }

    public Type Type { get; set; }

    public DataTemplate ProvideValue(IServiceProvider serviceProvider)
    {
        return new DataTemplate(() => AppServiceProvider.GetService(Type));
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<DataTemplate>).ProvideValue(serviceProvider);
    }
}

using Xamarin.Forms.Xaml;

namespace SyncMe.Lib.MarkupExtensions;

internal class ImageExtension : IMarkupExtension<ImageSource>
{
    public string SharedSource { get; set; }

    public ImageSource ProvideValue(IServiceProvider serviceProvider) => 
        SharedSource == null ? null : ImageSource.FromResource($"SyncMe.Lib.Resources.{SharedSource}");

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}

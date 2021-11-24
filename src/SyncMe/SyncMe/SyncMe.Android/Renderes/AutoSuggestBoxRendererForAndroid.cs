using Android.Content;
using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.Android;
using SyncMe.Droid.Renderes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AutoSuggestBox), typeof(AutoSuggestBoxRendererForAndroid))]
namespace SyncMe.Droid.Renderes;

public class AutoSuggestBoxRendererForAndroid : AutoSuggestBoxRenderer
{
    public AutoSuggestBoxRendererForAndroid(Context context) : base(context) { }

    protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
    {
        base.OnElementChanged(e);

        Control.SetTextCursorDrawable(Resource.Drawable.my_cursor);
    }
}

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using SyncMe.Droid.Renderes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryRendererForAndroid))]
namespace SyncMe.Droid.Renderes;

public class EntryRendererForAndroid : EntryRenderer
{
    public EntryRendererForAndroid(Context context) : base(context) {}

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        Control.SetTextCursorDrawable(Resource.Drawable.my_cursor);

        if (Control == null || e.NewElement == null) return;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Rgb(57, 194, 215));
        else
            Control.Background.SetColorFilter(Android.Graphics.Color.Rgb(57, 194, 215), PorterDuff.Mode.SrcAtop);
    }
}

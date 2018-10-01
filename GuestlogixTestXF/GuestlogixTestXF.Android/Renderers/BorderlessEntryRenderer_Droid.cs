using GuestlogixTestXF.Android;
using GuestlogixTestXF;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer_Droid))]
namespace GuestlogixTestXF.Android
{
    public class BorderlessEntryRenderer_Droid : EntryRenderer
    {
        public BorderlessEntryRenderer_Droid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}
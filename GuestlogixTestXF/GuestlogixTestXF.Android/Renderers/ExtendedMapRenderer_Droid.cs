using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using GuestlogixTestXF;
using GuestlogixTestXF.Android;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer_Droid))]
namespace GuestlogixTestXF.Android
{
    public class ExtendedMapRenderer_Droid : MapRenderer
    {
        public ExtendedMapRenderer_Droid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Here we can unsibscribe stuff if we want 
            }

            if (e.NewElement != null)
            {
                var map = (ExtendedMap)e.NewElement;
                routeCoordinates = map.RouteCoordinates;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            DrawRoute();
        }

        private void DrawRoute()
        {
            if (routeCoordinates?.Any() != true)
            {
                return;
            }

            var polylineOptions = new PolylineOptions();
            polylineOptions.InvokeColor(0x66FF0000);

            foreach (var position in routeCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            NativeMap.AddPolyline(polylineOptions);
        }

        private List<Position> routeCoordinates;
    }
}
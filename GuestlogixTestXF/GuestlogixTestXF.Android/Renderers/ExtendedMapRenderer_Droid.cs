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

		protected override MarkerOptions CreateMarker(Pin pin)
		{
			// Override this method to customize the pin for Android
			var marker = new MarkerOptions();
			marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
			marker.SetTitle(pin.Label);
			marker.SetSnippet(pin.Address);
			marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
			return marker;
		}

		private void DrawRoute()
        {
            if (routeCoordinates?.Any() != true)
            {
                return;
            }

            var polylineOptions = new PolylineOptions();

			polylineOptions.InvokeColor(System.Drawing.Color.Orange.ToArgb());

            foreach (var position in routeCoordinates)
            {
                polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
            }

            NativeMap.AddPolyline(polylineOptions);
        }

        private List<Position> routeCoordinates;
    }
}
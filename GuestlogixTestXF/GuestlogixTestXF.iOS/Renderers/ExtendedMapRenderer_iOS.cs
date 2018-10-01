using CoreLocation;
using GuestlogixTestXF;
using GuestlogixTestXF.iOS;
using MapKit;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer_iOS))]
namespace GuestlogixTestXF.iOS
{
	public class ExtendedMapRenderer_iOS : MapRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				if (nativeMap != null)
				{
					nativeMap.RemoveOverlays(nativeMap.Overlays);
					nativeMap.OverlayRenderer = null;
					polylineRenderer = null;
				}
			}

			if (e.NewElement != null)
			{
				var formsMap = (ExtendedMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				nativeMap.OverlayRenderer = GetOverlayRenderer;

				CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.RouteCoordinates.Count];
				int index = 0;
				foreach (var position in formsMap.RouteCoordinates)
				{
					coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
					index++;
				}

				var routeOverlay = MKPolyline.FromCoordinates(coords);
				nativeMap.AddOverlay(routeOverlay);
			}
		}

		MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
		{
			if (polylineRenderer == null && !Equals(overlayWrapper, null))
			{
				var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
				polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline)
				{
					FillColor = UIColor.Orange,
					StrokeColor = UIColor.Orange,
					LineWidth = 3,
					Alpha = 0.4f
				};
			}

			return polylineRenderer;
		}

		MKPolylineRenderer polylineRenderer;
	}
}

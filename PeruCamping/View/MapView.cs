using System;
using MapKit;
using CoreLocation;
using CoreGraphics;

namespace PeruCamping
{
	public class MapView : UIBaseView
	{
		MKMapView map;

		public MapView ()
		{
			Ini ();
		}

		public void Ini()
		{
			map = new MKMapView (new CGRect(0,0,888,490));

			//Map Style
			map.MapType = MKMapType.Satellite;

			Add(map);

			// set map center and region
			const double lat = 42.374260;
			const double lon = -71.120824;
			var mapCenter = new CLLocationCoordinate2D (lat, lon);
			var mapRegion = MKCoordinateRegion.FromDistance (mapCenter, 2000, 2000);
			map.CenterCoordinate = mapCenter;
			map.Region = mapRegion;

			// add an annotation
			map.AddAnnotation (new MKPointAnnotation {
				Title = "MyAnnotation", 
				Coordinate = new CLLocationCoordinate2D (42.364260, -71.120824)
			});


			map.ZoomEnabled = true;
			map.ScrollEnabled = true;
		}
	}
}


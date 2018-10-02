using GuestlogixTestXF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GuestlogixTestXF
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel(IEnumerable<Route> routes, IEnumerable<Airport> airports, string title)
        {
            this.routes = routes;
            this.airports = airports;
			this.Title = title;

            var airlineManager = ComponentContainer.Current.Resolve<IAirlineManager>();

            this.FlightInfoItems = new ObservableCollection<FlighInfoItemViewModel>();

			// Addid some information for the route
            airports.ToList().ForEach(airport =>
            {
                FlightInfoItems.Add(new FlighInfoItemViewModel()
                {
                    Airport = airport,
                    AirlineName = airlineManager.GetAirline(routes.FirstOrDefault(x => x.Origin == airport.IATA3 || x.Destination == airport.IATA3)?.AirlineId)?.Name ?? $"Missing airline name (Airline id {airport.IATA3})" 
                });
            });
        }

        private void FlightInfoSelected(string iata3)
        {
			// When user is clicking on flight info we pan to the airport in the map
            if (selectedFlightInfo == null)
            {
                return;
            }

            var airport = airports.FirstOrDefault(x => x.IATA3 == iata3);

            var lat = Convert.ToDouble(airport.Latitude.Trim(), CultureInfo.InvariantCulture);
            var lon = Convert.ToDouble(airport.Longitude.Trim(), CultureInfo.InvariantCulture);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lon), Distance.FromKilometers(100)));
        }

        private ICommand mapLoadedCommand;
        public ICommand MapLoadedCommand => mapLoadedCommand ?? (mapLoadedCommand = new Command((param) =>
        {
			// Command is fired when map is loaded. We add pins and polyline for the route
            if (!(param is ExtendedMap map))
            {
                return;
            }

            this.map = map;

            foreach (var airport in airports)
            {
                var lat = Convert.ToDouble(airport.Latitude.Trim(), CultureInfo.InvariantCulture);
                var lon = Convert.ToDouble(airport.Longitude.Trim(), CultureInfo.InvariantCulture);

                map.Pins.Add(new Pin()
                {
                    Label = airport.Name,
                    Type = PinType.Place,
                    Position = new Position(lat, lon)
                });

                map.RouteCoordinates.Add(new Position(lat, lon));
            }

            map.MoveToRegion(MapSpan.FromCenterAndRadius(map.RouteCoordinates.FirstOrDefault(), Distance.FromKilometers(1000)));
        }));

        private FlighInfoItemViewModel selectedFlightInfo;
        public FlighInfoItemViewModel SelectedFlightInfo
        {
            get { return selectedFlightInfo; }
			set 
			{ 
				selectedFlightInfo = value;

				if (selectedFlightInfo == null)
				{
					return;
				}

				FlightInfoSelected(selectedFlightInfo.Airport.IATA3);

				selectedFlightInfo = null;
			}
        }
        
        public ObservableCollection<FlighInfoItemViewModel> FlightInfoItems { get; private set; }

		public string Title
		{
			get;
			set;
		}

        private ExtendedMap map;

        private readonly IEnumerable<Route> routes;
        private readonly IEnumerable<Airport> airports;
    }

    public class FlighInfoItemViewModel : INotifyPropertyChanged
    {
        public string AirlineName { get; set; }
        public Airport Airport { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using GuestlogixTestXF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GuestlogixTestXF
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
            // We will run it in a background task so we don't hang up the UI and loading time
            Task.Run(() =>
            {
                // Init the managers from container
                routeManager = ComponentContainer.Current.Resolve<IRouteManager>();
                airportManager = ComponentContainer.Current.Resolve<IAirportManager>();

                // Get all airpots
                airports = airportManager.GetAirports();

                // Create the list items here to reduce time. Will be used to search for 
                // origin and destination
                listItems = airports.Select(x => new ListItemViewModel()
                {
                    DisplayName = $"{x.Name}, {x.City}, {x.IATA3}",
                    Id = x.IATA3 ?? x.Name,
                    Tags = x.Name + x.City + x.IATA3 + x.Country
                });
            });
        }

        private ICommand selectOriginCommand;
        public ICommand SelectOriginCommand => selectOriginCommand ?? (selectOriginCommand = new Command(async () =>
        {
            // We subscribe to any changes for origin selection
            MessagingCenter.Instance.Subscribe<ListViewModel, ListItemViewModel>(this, AppConfig.MESSAGE_KEY_ITEM_SELECTED, ((sender, item) =>
            {
                if (item != null)
                {
                    SelectedOriginAirport = airports.FirstOrDefault(x => x.IATA3 == item.Id);
                }

                MessagingCenter.Instance.Unsubscribe<ListViewModel, ListItemViewModel>(this, AppConfig.MESSAGE_KEY_ITEM_SELECTED);
            }));

            while (listItems?.Any() != true)
            {
                await Task.Delay(TimeSpan.FromSeconds(.5));
            }

            var viewModel = new ListViewModel(listItems, "Select origin");
            await Navigation.PushModalAsync(new NavigationPage(ViewContainer.Current.CreatePage(viewModel)));
        }));

        private ICommand selectDestinationCommand;
        public ICommand SelectDestinationCommand => selectDestinationCommand ?? (selectDestinationCommand = new Command(async () =>
        {
            // We subscribe to any changes for origin selection
            MessagingCenter.Instance.Subscribe<ListViewModel, ListItemViewModel>(this, AppConfig.MESSAGE_KEY_ITEM_SELECTED, ((sender, item) =>
            {
                if (item != null)
                {
                    SelectedDestinationAirport = airports.FirstOrDefault(x => x.IATA3 == item.Id);
                }

                MessagingCenter.Instance.Unsubscribe<ListViewModel, ListItemViewModel>(this, AppConfig.MESSAGE_KEY_ITEM_SELECTED);
            }));

            while (listItems?.Any() != true)
            {
                await Task.Delay(TimeSpan.FromSeconds(.5));
            }

            var viewModel = new ListViewModel(listItems, "Select destination");
            await Navigation.PushModalAsync(new NavigationPage(ViewContainer.Current.CreatePage(viewModel)));
        }));

        private ICommand searchCommand;
        public ICommand SearchCommand => searchCommand ?? (searchCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            if (SelectedOriginAirport == null)
            {
                // No origin airport was found
                ShowAlert("", "");
                return;
            }

            if (SelectedDestinationAirport == null)
            {
                // No destination airport was found
                ShowAlert("", "");
                return;
            }

            try
            {
                IsBusy = true;

                IEnumerable<Route> routes = null;

                ShowLoading("Finding a route");

                await Task.Run(() =>
                {
                    routes = routeManager.GetRoutes(SelectedOriginAirport.IATA3, SelectedDestinationAirport.IATA3);
                });

                if (routes?.Any() != true)
                {
                    HideLoading();
                    ShowAlert("", "");
                    return;
                }

                var distinctCodes = new List<string>();

                routes.ToList().ForEach(x =>
                {
                    if (!distinctCodes.Contains(x.Origin))
                    {
                        distinctCodes.Add(x.Origin);
                    }

                    if (!distinctCodes.Contains(x.Destination))
                    {
                        distinctCodes.Add(x.Destination);
                    }
                });

                var airportsInRoute = airportManager.GetAirports(distinctCodes);

                var viewModel = new MapViewModel(routes, airportsInRoute);

                await Navigation.PushAsync(ViewContainer.Current.CreatePage(viewModel));
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                HideLoading();
                IsBusy = false;
            }
        }));

        public Airport SelectedOriginAirport { get; set; }
        public Airport SelectedDestinationAirport { get; set; }

        private IEnumerable<Airport> airports;
        private IEnumerable<ListItemViewModel> listItems;

        private IRouteManager routeManager;
        private IAirportManager airportManager;
    }
}

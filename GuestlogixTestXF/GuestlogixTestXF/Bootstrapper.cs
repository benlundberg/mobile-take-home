using GuestlogixTestXF.Core;

namespace GuestlogixTestXF
{
    public class Bootstrapper
    {
        public static void RegisterTypes()
        {
            // Helpers
            ComponentContainer.Current.Register<ITranslateHelper, TranslateHelper>();
            ComponentContainer.Current.Register<INetworkStatusHelper, NetworkStatusHelper>(singelton: true);
            ComponentContainer.Current.Register<IFileHelper, FileHelper>(singelton: true);

            // Managers
            ComponentContainer.Current.Register<IAirlineManager, AirlineManager>();
            ComponentContainer.Current.Register<IAirportManager, AirportManager>();
            ComponentContainer.Current.Register<IRouteManager, RouteManager>();
        }

        public static void RegisterViews()
        {
            ViewContainer.Current.Register<SearchViewModel, SearchPage>();
            ViewContainer.Current.Register<ListViewModel, ListPage>();
            ViewContainer.Current.Register<MapViewModel, MapPage>();
        }
    }
}

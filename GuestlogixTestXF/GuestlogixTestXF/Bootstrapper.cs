using GuestlogixTestXF.Core;
using System;
using System.Collections.Generic;

namespace GuestlogixTestXF
{
    public class Bootstrapper
    {
        public static void RegisterTypes()
        {
            // Repositories
            ComponentContainer.Current.Register<IDatabaseRepository, DatabaseRepository>(singelton: true);

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
            ViewContainer.Current.Register<HomeMasterViewModel, HomeMasterPage>();
            ViewContainer.Current.Register<MasterViewModel, MasterPage>();

            ViewContainer.Current.Register<SearchViewModel, SearchPage>();
            ViewContainer.Current.Register<ListViewModel, ListPage>();
            ViewContainer.Current.Register<MapViewModel, MapPage>();
        }

        public static void CreateTables()
        {
            ComponentContainer.Current.Resolve<IDatabaseRepository>().CreateTablesAsync(new List<Type>()
            {
            });
        }
    }
}

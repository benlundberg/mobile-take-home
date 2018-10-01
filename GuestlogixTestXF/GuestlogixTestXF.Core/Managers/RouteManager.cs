using System.Collections.Generic;
using System.Linq;

namespace GuestlogixTestXF.Core
{
    public class RouteManager : IRouteManager
    {
        public RouteManager(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        /// <summary>
        /// Returns all routes from the CSV-file
        /// </summary>
        public IEnumerable<Route> GetRoutes()
        {
            IEnumerable<Route> routes = null;

            using (var stream = fileHelper.ReadResourceFileAsStream("GuestlogixTestXF.Data.routes.csv"))
            {
                routes = CsvConverter.Serialize<Route>(stream);
            }

            return routes;
        }

        /// <summary>
        /// Gets the route or routes if more between an origin and destination.
        /// </summary>
        public IEnumerable<Route> GetRoutes(string origin, string destination)
        {
            var routes = GetRoutes();

            var foundRoutes = routes.Where(x => x.Origin == origin && x.Destination == destination);

            // Check if there was a route
            if (foundRoutes?.Any() != true)
            {
                foundRoutes = SearchOptionalRoute(destination, routes.Where(x => x.Origin == origin), routes, new List<string>() { origin });
            }

            return foundRoutes;
        }

        /// <summary>
        /// Search and will return a multi-route between origin and final destination.
        /// </summary>
        public IEnumerable<Route> SearchOptionalRoute(string finalDestination, IEnumerable<Route> destinations, IEnumerable<Route> routes, List<string> exceptRoute)
        {
            List<Route> foundRoutes = new List<Route>();

            foreach (var destination in destinations)
            {
                var dest = GetDestinations(destination.Destination, routes).Where(x => !exceptRoute.Contains(x.Origin));

                if (dest?.Any() != true)
                {
                    continue;
                }

                if (dest.Select(x => x.Destination).Contains(finalDestination))
                {
                    foundRoutes.Add(destination);

                    var finalRoute = dest.FirstOrDefault(x => x.Destination == finalDestination);

                    foundRoutes.Add(finalRoute);

                    break;
                }
                else
                {
                    exceptRoute.Add(destination.Origin);
                    var route = SearchOptionalRoute(finalDestination, dest, routes, exceptRoute);

                    if (route?.Any() != true)
                    {
                        foundRoutes.AddRange(route);
                        break;
                    }
                }
            }

            return foundRoutes;
        }

        /// <summary>
        /// Returns all destinations from a specific origin
        /// </summary>
        public IEnumerable<Route> GetDestinations(string origin, IEnumerable<Route> routes)
        {
            return routes.Where(x => x.Origin == origin);
        }

        private IFileHelper fileHelper;
    }
}

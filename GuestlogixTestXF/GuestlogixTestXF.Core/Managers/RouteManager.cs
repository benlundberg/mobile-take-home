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

            List<Route> foundRoutes = routes.Where(x => x.Origin == origin && x.Destination == destination).ToList();

            // Check if there was a route
            if (foundRoutes?.Any() != true)
            {
				// No route was found so we try find an alternative one
				var allPossibleRoutes = GetAllPossibleRoutes(destination, origin, routes);

				// Check if we found an possible route
				if (allPossibleRoutes?.Any(x => x?.Any() == true) == true)
				{
					// Get the quickest route
					int min = allPossibleRoutes.Min(x => x.Count());
					foundRoutes = allPossibleRoutes.FirstOrDefault(x => x.Count() == min).ToList();

					// We make a last check to see if the first route is in the list
					var firstRoute = routes.FirstOrDefault(x => x.Origin == origin && x.Destination == foundRoutes?.FirstOrDefault()?.Origin);

					if (firstRoute != null)
					{
						if (foundRoutes?.FirstOrDefault().Origin != firstRoute.Origin &&
						    foundRoutes?.FirstOrDefault().Destination != firstRoute.Destination)
						{
							foundRoutes.Insert(0, firstRoute);
						}
					}
				}
			}

            return foundRoutes;
        }

		public IEnumerable<IEnumerable<Route>> GetAllPossibleRoutes(string finalDestination, string origin, IEnumerable<Route> routes, List<string> exceptRoute = null, List<IEnumerable<Route>> allPossibleRoutes = null)
		{
			// List to store all possible routes
			if (allPossibleRoutes == null)
			{
				allPossibleRoutes = new List<IEnumerable<Route>>();
			}

			// Routes we already have
			if (exceptRoute == null)
			{
				exceptRoute = new List<string>();
			}

			// Get optional route
			var foundRoutes = SearchOptionalRoute(finalDestination, routes.Where(x => x.Origin == origin), routes, exceptRoute);

			// If we found one we add it to the list, otherwise we return the routes
			if (foundRoutes?.Any() == true)
			{
				allPossibleRoutes.Add(foundRoutes);

				exceptRoute.Add(foundRoutes.FirstOrDefault()?.Destination);

				// Run this method again
				var moreRoutes = GetAllPossibleRoutes(finalDestination, origin, routes, exceptRoute, allPossibleRoutes);

				allPossibleRoutes.AddRange(moreRoutes);
			}

			return allPossibleRoutes;
		}

        /// <summary>
        /// Search and will return a multi-route between origin and final destination.
        /// </summary>
        public IEnumerable<Route> SearchOptionalRoute(string finalDestination, IEnumerable<Route> destinations, IEnumerable<Route> routes, List<string> exceptRoute)
        {
            List<Route> foundRoutes = new List<Route>();

			// Go through the destinations to for the origin
            foreach (var destination in destinations)
            {
				// Get destinations where this destination can go, filter away the airports we already looked at
                var dest = GetDestinations(destination.Destination, routes).Where(x => !exceptRoute.Contains(x.Origin));

                if (dest?.Any() != true)
                {
                    continue;
                }

				// Check if we have the final destination
                if (dest.Select(x => x.Destination).Contains(finalDestination))
                {
                    foundRoutes.Add(destination);

                    var finalRoute = dest.FirstOrDefault(x => x.Destination == finalDestination);

                    foundRoutes.Add(finalRoute);

                    break;
                }
                else
                {
					// If we haven't found the final destination we try this method again 
                    exceptRoute.Add(destination.Origin);
                    
					var route = SearchOptionalRoute(finalDestination, dest, routes, exceptRoute);

                    if (route?.Any() == true)
                    {
						foreach (var r in route)
						{
							if (foundRoutes?.Any(x => x.Origin == r.Origin && x.Destination == r.Destination) != true)
							{
								foundRoutes.Add(r);
							}
						}
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

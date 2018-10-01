using System.Collections.Generic;

namespace GuestlogixTestXF.Core
{
    public interface IRouteManager
    {
        IEnumerable<Route> GetRoutes();
        IEnumerable<Route> GetRoutes(string origin, string destination);
    }
}

using System.Collections.Generic;

namespace GuestlogixTestXF.Core
{
    public interface IAirportManager
    {
        IEnumerable<Airport> GetAirports();
        IEnumerable<Airport> GetAirports(IEnumerable<string> codes);
    }
}

using System.Collections.Generic;

namespace GuestlogixTestXF.Core
{
    public interface IAirlineManager
    {
        IEnumerable<Airline> GetAirlines();
        Airline GetAirline(string airlineId);
    }
}

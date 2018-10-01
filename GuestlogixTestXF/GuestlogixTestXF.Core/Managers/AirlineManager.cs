using System.Collections.Generic;
using System.Linq;

namespace GuestlogixTestXF.Core
{
    public class AirlineManager : IAirlineManager
    {
        public AirlineManager(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public IEnumerable<Airline> GetAirlines()
        {
            IEnumerable<Airline> airlines = null;

            using (var stream = fileHelper.ReadResourceFileAsStream("GuestlogixTestXF.Data.airlines.csv"))
            {
               airlines = CsvConverter.Serialize<Airline>(stream);
            }

            return airlines;
        }

        public Airline GetAirline(string airlineId)
        {
            var airlines = GetAirlines();

            return airlines.FirstOrDefault(x => x.DigitCode2 == airlineId || x.DigitCode3 == airlineId);
        }

        private IFileHelper fileHelper;
    }
}

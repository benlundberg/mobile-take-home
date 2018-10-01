using System.Collections.Generic;
using System.Linq;

namespace GuestlogixTestXF.Core
{
    public class AirportManager : IAirportManager
    {
        public AirportManager(IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public IEnumerable<Airport> GetAirports()
        {
            IEnumerable<Airport> airports = null;

            using (var stream = fileHelper.ReadResourceFileAsStream("GuestlogixTestXF.Data.airports.csv"))
            {
                airports = CsvConverter.Serialize<Airport>(stream);
            }

            return airports;
        }

        /// <summary>
        /// Returns airpots by IATA3 codes
        /// </summary>
        public IEnumerable<Airport> GetAirports(IEnumerable<string> codes)
        {
            var airports = GetAirports();

            var list = new List<Airport>();

            // To get it in correct order we do a foreach loop
            foreach (var code in codes)
            {
                var airport = airports.FirstOrDefault(x => x.IATA3 == code);

                list.Add(airport);
            }

            return list;
        }

        private IFileHelper fileHelper;
    }
}

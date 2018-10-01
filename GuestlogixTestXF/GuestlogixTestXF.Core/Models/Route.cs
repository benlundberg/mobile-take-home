namespace GuestlogixTestXF.Core
{
    public class Route
    {
        [Csv(CsvName = "Airline Id")]
        public string AirlineId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}

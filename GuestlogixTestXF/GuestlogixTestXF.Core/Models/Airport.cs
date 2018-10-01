namespace GuestlogixTestXF.Core
{
    public class Airport
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Csv(CsvName = "IATA 3")]
        public string IATA3 { get; set; }
        [Csv(CsvName = "Latitute")]
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}

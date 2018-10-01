namespace GuestlogixTestXF.Core
{
    public class Airline
    {
        public string Name { get; set; }
        [Csv(CsvName = "2 Digit Code")]
        public string DigitCode2 { get; set; }
        [Csv(CsvName = "3 Digit Code")]
        public string DigitCode3 { get; set; }
        public string Country { get; set; }
    }
}

namespace MABS.Application.Common.Geolocation
{
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCoordinates(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}

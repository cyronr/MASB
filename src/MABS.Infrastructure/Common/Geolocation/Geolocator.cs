using MABS.Application.Common.Geolocation;
using MABS.Application.Common.Http;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;
using Nest;
using NetTopologySuite.Geometries;
using Newtonsoft.Json.Linq;

namespace MABS.Infrastructure.Common.Geolocation
{
    public class Geolocator : IGeolocator
    {
        private readonly ILogger<Geolocator> _logger;
        private readonly IHttpRequester _httpRequester;

        public Geolocator(ILogger<Geolocator> logger, IHttpRequester httpRequester)
        {
            _logger = logger;
            _httpRequester = httpRequester;
        }

        public double CalculateDistanceBetweenPoints(GeoCoordinates point1, GeoCoordinates point2)
        {
            var geoPoint1 = new Point(point1.Longitude, point1.Latitude);
            var geoPoint2 = new Point(point2.Longitude, point2.Latitude);

            return Haversine.Distance(geoPoint1, geoPoint2, SpatialReferenceSystem.WGS84);
        }

        public async Task<GeoCoordinates> EncodeAddress(Address address)
        {
            _logger.LogDebug("Getting address coordinates.");

            var url = $"https://nominatim.openstreetmap.org/search/{Uri.EscapeDataString(GetStringAddress(address))}?format=json&addressdetails=1&limit=1&polygon_svg=1";
            var headers = new Dictionary<string, string>(){
                { "User-Agent", "Other" }
            };

            var response = await _httpRequester.HttpGet(url, headers);

            if (!response.IsSuccessStatusCode)
                throw new WrongAddressException("Wrong address!", "Address not found");

            var responseData = await response.Content.ReadAsStringAsync();
            if (responseData == "[]")
                throw new WrongAddressException("Wrong address!", "Address not found");

            JArray geocodeData = JArray.Parse(responseData);
            return new GeoCoordinates
            {
                Latitude = (double)geocodeData[0]["lat"],
                Longitude = (double)geocodeData[0]["lon"]
            };
        }

        private string GetStringAddress(Address address)
        {
            return $"{address.PostalCode} {address.City}, {address.StreetName} {address.HouseNumber}";
        }
    }
}

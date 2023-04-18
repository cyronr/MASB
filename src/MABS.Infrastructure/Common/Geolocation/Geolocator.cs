using MABS.Application.Common.Geolocation;
using MABS.Application.Common.Http;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;
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
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
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
            return new GeoCoordinates((double)geocodeData[0]["lat"], (double)geocodeData[0]["lon"]);
        }

        private string GetStringAddress(Address address)
        {
            return $"{address.PostalCode} {address.City}, {address.StreetName} {address.HouseNumber}";
        }
    }
}

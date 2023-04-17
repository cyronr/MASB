using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.Common.Geolocation
{
    public interface IGeolocator
    {
        Task<GeoCoordinates> EncodeAddress(Address address);
        double CalculateDistanceBetweenPoints(GeoCoordinates point1, GeoCoordinates point2);
    }
}

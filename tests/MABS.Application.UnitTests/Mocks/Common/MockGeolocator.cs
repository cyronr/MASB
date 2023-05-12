using MABS.Application.Common.Geolocation;
using MABS.Application.Common.Http;

namespace MABS.Application.UnitTests.Mocks.Common;

public static class MockGeolocator
{
    public static Mock<IGeolocator> GetGeolocator()
    {
        var mockHttpRequester = new Mock<IGeolocator>();

        mockHttpRequester.Setup(r => r.EncodeAddress(It.IsAny<Address>()))
            .ReturnsAsync(new GeoCoordinates(1.0, 1.0));
        mockHttpRequester.Setup(r => r.CalculateDistanceBetweenPoints(It.IsAny<GeoCoordinates>(), It.IsAny<GeoCoordinates>()))
            .Returns(1.0);

        return mockHttpRequester;
    }
}

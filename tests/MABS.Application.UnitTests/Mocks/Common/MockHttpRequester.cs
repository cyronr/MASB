using MABS.Application.Common.Http;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.UnitTests.Mocks.Common;

public static class MockHttpRequester
{
    public static Mock<IHttpRequester> GetHttpRequester()
    {
        var mockHttpRequester = new Mock<IHttpRequester>();

        mockHttpRequester.Setup(r => r.HttpGet(It.IsAny<string>())).Verifiable();
        mockHttpRequester.Setup(r => r.HttpGet(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Verifiable();

        return mockHttpRequester;
    }
}

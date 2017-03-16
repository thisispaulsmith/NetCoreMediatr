using NetCoreMediatr;
using Shouldly;
using Xunit;

namespace Tests
{
    public class ValuesControllerTests
    {
        private readonly WebServerTestFixture<Startup> _fixture;

        public ValuesControllerTests()
        {
            _fixture = new WebServerTestFixture<Startup>(services =>
            {
                // Optionally replace any services, currently not possible with Mediators as AddMediatR 
                // uses brute force approach and overwrites services
            });
        }

        [Fact]
        public void list()
        {
            // When
            var response = _fixture.Get("/api/values");

            // Then
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        }
    }
}

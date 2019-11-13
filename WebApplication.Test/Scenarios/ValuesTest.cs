using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication.Test.Fixtures;
using Xunit;

namespace WebApplication.Test.Scenarios
{
    public class ValuesTest
    {

        private readonly TestContext _testContext;
        public ValuesTest()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task Values_Get_ReturnsOkResponse()
        {
            HttpResponseMessage response = await _testContext.Client.GetAsync("/api/values");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Values_GetById_ValuesReturnsOkResponse()
        {
            HttpResponseMessage response = await _testContext.Client.GetAsync("/api/values/5");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Values_GetById_ReturnsBadRequestResponse()
        {
            HttpResponseMessage response = await _testContext.Client.GetAsync("/api/values/XXX");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest,"because the data is invalid");
        }

        [Fact]
        public async Task Values_GetById_CorrectContentType()
        {
            HttpResponseMessage response = await _testContext.Client.GetAsync("/api/values/5");
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be("text/plain; charset=utf-8");
        }


    }
}

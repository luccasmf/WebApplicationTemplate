using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication.Test.Fixtures;
using WebApplicationTemplate.Security;
using Xunit;

namespace WebApplication.Test.Scenarios
{
    public class AccountTest
    {
        private readonly TestContext _testContext;
        public AccountTest()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task Account_Login()
        {
            User usr = new User
            {
                UserID = "123",
                Password = "123"
            };
            StringContent stringContent = new StringContent(JsonSerializer.Serialize(usr), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _testContext.Client.PostAsync("/api/Account/Login", stringContent);
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);


        }
    }
}

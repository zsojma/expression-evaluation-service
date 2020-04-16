using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ExpressionEvaluation.Api.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("1-1", 0)]
        [InlineData("((1+2)*43)/3.14+2^3", 49.0828025478)]
        [InlineData("-((1+(2^(1-2)^3))*-43)^-3", 3.7267e-6)]
        public async Task Compute_Get_ReturnsCorrectResult(string expression, double expectedResult)
        {
            // Arrange
            var client = _factory.CreateClient();
            var encodedExpr = HttpUtility.UrlEncode(expression);

            // Act
            var response = await client.GetAsync("/compute?expr=" + encodedExpr);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var resultStr = await response.Content.ReadAsStringAsync();
            Assert.True(double.TryParse(resultStr, out var result));

            Assert.Equal(expectedResult, result, 10);
        }

        [Theory]
        [InlineData("((1+2)")]
        [InlineData("1+a")]
        public async Task Compute_Get_ReturnsErrorMessage(string expression)
        {
            // Arrange
            var client = _factory.CreateClient();
            var encodedExpr = HttpUtility.UrlEncode(expression);

            // Act
            var response = await client.GetAsync("/compute?expr=" + encodedExpr);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var resultStr = await response.Content.ReadAsStringAsync();
            Assert.NotNull(resultStr);
        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace API.Tests
{
    public class ProductControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response =
                await _client.GetAsync(
                    "/api/v1/products");

            // Assert
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_ShouldReturnSuccessOrNotFound()
        {
            // Act
            var response =
                await _client.GetAsync(
                    "/api/v1/products/1");

            // Assert
            response.StatusCode
                .Should()
                .BeOneOf(
                    HttpStatusCode.OK,
                    HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_WithoutToken_ShouldReturnUnauthorized()
        {
            // Arrange
            var json = """
            {
                "productName": "Laptop"
            }
            """;

            var content =
                new StringContent(
                    json,
                    System.Text.Encoding.UTF8,
                    "application/json");

            // Act
            var response =
                await _client.PostAsync(
                    "/api/v1/products",
                    content);

            // Assert
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_WithoutToken_ShouldReturnUnauthorized()
        {
            // Act
            var response =
                await _client.DeleteAsync(
                    "/api/v1/products/1");

            // Assert
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }
    }
}
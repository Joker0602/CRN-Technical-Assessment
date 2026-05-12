using API.Controllers;
using Application.DTOs;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class ItemControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ItemControllerTests(
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
                    "/api/v1/items");

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
                    "/api/v1/items/1");

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
                "productId": 1,
                "quantity": 5
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
                    "/api/v1/items",
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
                    "/api/v1/items/1");

            // Assert
            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }
    }
}


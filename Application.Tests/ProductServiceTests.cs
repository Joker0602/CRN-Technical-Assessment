using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;

        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();

            _service = new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProducts()
        {
            // Arrange
            int pageNumber = 1;

            int pageSize = 10;

            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    ProductName = "Laptop",
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.UtcNow
                }
            };

            _repositoryMock
                .Setup(x => x.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(products);

            // Act
            var result =
                await _service.GetAllAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result =
                await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();

            result!.Id.Should().Be(1);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateProduct()
        {
            // Arrange
            var dto = new CreateProductDto
            {
                ProductName = "Laptop"
            };

            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();

            result.ProductName.Should().Be("Laptop");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                ProductName = "Laptop"
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(product);

            _repositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }
    }
}

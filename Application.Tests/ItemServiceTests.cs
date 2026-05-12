using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Application.Tests
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _repositoryMock;

        private readonly ItemService _service;

        public ItemServiceTests()
        {
            _repositoryMock = new Mock<IItemRepository>();

            _service = new ItemService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnItems()
        {
            // Arrange
            int pageNumber = 1;

            int pageSize = 10;

            var items = new List<Item>
            {
                new Item
                {
                    Id = 1,
                    ProductId = 1,
                    Quantity = 5
                }
            };

            _repositoryMock
                .Setup(x => x.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(items);

            // Act
            var result =
                await _service.GetAllAsync(
                    pageNumber,
                    pageSize);

            // Assert
            result.Should().NotBeNull();

            result.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem()
        {
            // Arrange
            var item = new Item
            {
                Id = 1,
                ProductId = 1,
                Quantity = 5
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

            // Act
            var result =
                await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();

            result!.Id.Should().Be(1);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            // Arrange
            var dto = new CreateItemDto
            {
                ProductId = 1,
                Quantity = 10
            };

            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Item>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();

            result.Quantity.Should().Be(10);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue()
        {
            // Arrange
            var item = new Item
            {
                Id = 1,
                ProductId = 1,
                Quantity = 5
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item);

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
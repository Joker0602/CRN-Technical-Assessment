using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests
{
    public class ItemRepositoryTests
    {
        private readonly ApplicationDbContext _context;

        private readonly ItemRepository _repository;

        public ItemRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            _context =
                new ApplicationDbContext(options);

            _repository =
                new ItemRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddItem()
        {
            // Arrange

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            var item = new Item
            {
                ProductId = product.Id,
                Quantity = 5
            };

            // Act
            await _repository.AddAsync(item);

            await _repository.SaveChangesAsync();

            var result =
                await _context.Items.FirstOrDefaultAsync();

            // Assert
            result.Should().NotBeNull();

            result!.Quantity.Should().Be(5);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItem()
        {
            // Arrange

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            var item = new Item
            {
                ProductId = product.Id,
                Quantity = 10
            };

            _context.Items.Add(item);

            await _context.SaveChangesAsync();

            // Act
            var result =
                await _repository.GetByIdAsync(item.Id);

            // Assert
            result.Should().NotBeNull();

            result!.Quantity.Should().Be(10);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnItems()
        {
            // Arrange

            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            _context.Items.AddRange(
                new Item
                {
                    ProductId = product.Id,
                    Quantity = 5
                },
                new Item
                {
                    ProductId = product.Id,
                    Quantity = 15
                });

            await _context.SaveChangesAsync();

            // Act
            var result =
                await _repository.GetAllAsync(1, 10);

            // Assert
            result.Should().NotBeNull();

            result.Count().Should().Be(2);
        }
    }
}

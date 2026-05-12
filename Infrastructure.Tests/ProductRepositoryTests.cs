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
    public class ProductRepositoryTests
    {
        private readonly ApplicationDbContext _context;

        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString())
                .Options;

            _context =
                new ApplicationDbContext(options);

            _repository =
                new ProductRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Laptop",
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            // Act
            await _repository.AddAsync(product);

            await _repository.SaveChangesAsync();

            var result =
                await _context.Products.FirstOrDefaultAsync();

            // Assert
            result.Should().NotBeNull();

            result!.ProductName.Should().Be("Laptop");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
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

            // Act
            var result =
                await _repository.GetByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
        }
    }
}

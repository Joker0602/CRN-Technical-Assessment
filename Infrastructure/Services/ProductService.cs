using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var products = await _repository.GetAllAsync(pageNumber, pageSize);

            return products.Select(x => new ProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedBy = product.CreatedBy,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            product.ProductName = dto.ProductName;
            product.ModifiedBy = "Admin";
            product.ModifiedOn = DateTime.UtcNow;

            _repository.Update(product);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            _repository.Delete(product);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}

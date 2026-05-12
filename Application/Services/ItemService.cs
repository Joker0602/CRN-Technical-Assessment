using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;

        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var items = await _repository.GetAllAsync(pageNumber, pageSize);

            return items.Select(x => new ItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity
            });
        }

        public async Task<ItemDto?> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
                return null;

            return new ItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var item = new Item
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _repository.AddAsync(item);

            await _repository.SaveChangesAsync();

            return new ItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateItemDto dto)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
                return false;

            item.Quantity = dto.Quantity;
            item.ProductId = dto.ProductId;
            _repository.Update(item);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null)
                return false;

            _repository.Delete(item);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}

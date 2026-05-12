using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync(int pageNumber, int pageSize);

        Task<ItemDto?> GetByIdAsync(int id);

        Task<ItemDto> CreateAsync(CreateItemDto dto);

        Task<bool> UpdateAsync(int id, UpdateItemDto dto);

        Task<bool> DeleteAsync(int id);
    }
}

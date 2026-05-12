using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync(int pageNumber, int pageSize);

        Task<Item?> GetByIdAsync(int id);

        Task AddAsync(Item item);

        void Update(Item item);

        void Delete(Item item);

        Task SaveChangesAsync();
    }
}

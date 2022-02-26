using Microsoft.EntityFrameworkCore;
using RandApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandApp.Repositories.Abstraction
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<bool> CreateAsync(T item);
        Task<IEnumerable<T>> ReadAsync();
        Task<T> ReadByIdAsync(int? id);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);
        Task<bool> SaveChangesAsync();
        DbSet<T> Get();
    }
}

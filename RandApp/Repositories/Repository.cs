using Microsoft.EntityFrameworkCore;
using RandApp.Data;
using RandApp.Models;
using RandApp.Repositories.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandApp.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _db = default;
        private readonly DbSet<T> _entity = default;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _entity = _db.Set<T>();
        }
        public async Task<bool> CreateAsync(T item)
        {
            _entity.Add(item);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(T item)
        {
            _entity.Remove(item);
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> ReadAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<T> ReadByIdAsync(int? id)
        {
            return await _entity.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> UpdateAsync(T item)
        {
            _entity.Update(item);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return (await _db.SaveChangesAsync()) >= 0;
            }
            catch
            {
                return false;
            }
        }

        public DbSet<T> Get()
        {
            return _entity;
        }
    }
}

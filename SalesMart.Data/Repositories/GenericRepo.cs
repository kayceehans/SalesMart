using Microsoft.EntityFrameworkCore;
using SalesMart.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesMart.Data.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly SalesMartContext context = null;
        private readonly DbSet<T> table = null;

        public GenericRepo(SalesMartContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public Task<List<T>> GetAllAsync()
        {
            return table.ToListAsync();
        }

        public T GetById(int id)
        {
            return table.Find(id);
        }
        public async Task<long> Getcount(string name)
        {
            long count = await table.CountAsync();
            return count;
        }

        public T GetByIdWithIncludes(int id)
        {
            return table
                .Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> GetByIdWithIncludesAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public bool Remove(int id)
        {
            try
            {
                var product = table.Find(id);
                if (product is { })
                {
                    table.Remove(product);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public void Add(in T sender)
        {
            table.Add(sender).State = EntityState.Added;
        }

        public void Update(in T sender)
        {
            table.Entry(sender).State = EntityState.Modified;
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return context.SaveChangesAsync();
        }

        public T Select(Expression<Func<T, bool>> predicate)
            => table
                .Where(predicate)
                .FirstOrDefault()!;

        public async Task<T> SelectAsync(Expression<Func<T, bool>> predicate)
            =>
            (
                await table
                    .Where(predicate)
                    .FirstOrDefaultAsync()
            )!;

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

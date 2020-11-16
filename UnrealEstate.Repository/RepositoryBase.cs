using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnrealEstate.Data.EF;

namespace UnrealEstate.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        
        protected UnrealEstateDbContext _context { get; set; }

        public RepositoryBase(UnrealEstateDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> FindByIdAsync(int Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }
    }
}

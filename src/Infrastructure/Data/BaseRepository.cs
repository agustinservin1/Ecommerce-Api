﻿using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class BaseRepository <T> : IBaseRepository<T> where T : class 
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {  
            _context = context; 
        }
    
        public async Task Create (T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete (T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task <IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateRange(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);  // Usamos el método UpdateRange para múltiples entidades
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return await query.AsNoTracking().ToListAsync();
        }

    }
}

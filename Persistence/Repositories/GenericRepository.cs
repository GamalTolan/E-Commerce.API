using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
            =>await _context.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(Specification<TEntity> specification)
            => await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).CountAsync();


        public void Delete(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool IsTrackable = false)
        {
            if(IsTrackable)
                return await _context.Set<TEntity>().ToListAsync(); 
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specification)
        {
           return await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        => await _context.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> GetByIdAsync(Specification<TEntity> specification)
        {
            return await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);

    }
}

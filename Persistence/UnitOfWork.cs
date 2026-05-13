using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ConcurrentDictionary<string,object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
                 => (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, x => new GenericRepository<TEntity, TKey>(_context));


        public async Task SaveChangesAsync()
         => await _context.SaveChangesAsync();
    }
}
 
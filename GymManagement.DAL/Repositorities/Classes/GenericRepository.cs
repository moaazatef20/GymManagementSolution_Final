using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositorities.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _Dbcontext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            _Dbcontext = dbContext;
            _dbSet = _Dbcontext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate, ct);
        }

       
        public Task DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id, ct);
        }

        public Task UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public  Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
        {
           return predicate is null ? _dbSet.AsNoTracking().CountAsync(ct) : _dbSet.AsNoTracking().CountAsync(predicate, ct);
        }

    }
}
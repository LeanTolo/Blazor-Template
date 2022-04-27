using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private IBaseRepository<TEntity> _baseRepository;

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _baseRepository.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _baseRepository.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                    string includeProperties = "")
        {
            return await _baseRepository.GetAsync(filter, orderBy, includeProperties);
        }

        public async ValueTask<TEntity> GetByIdAsync(int id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _baseRepository.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _baseRepository.RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _baseRepository.SingleOrDefaultAsync(predicate);
        }

        public async Task Update(int id, TEntity entityToUpdate)
        {
            await _baseRepository.Update(entityToUpdate);
        }

        public async Task UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
        {
            await _baseRepository.UpdateRange(entitiesToUpdate);
        }

    }
}

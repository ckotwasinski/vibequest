using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using VibeQuest.Model;

namespace VibeQuest.DataAccess.Infrastructure
{
    public abstract class Repository<T, TContext> : IRepository<T> where T : class where TContext : DbContext
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly IUnitOfWork<TContext> _unitOfWork;

        public Repository(IUnitOfWork<TContext> unitOfWork)
        {
            _dbContext = unitOfWork.Context ?? throw new ArgumentException(nameof(unitOfWork.Context));
            _dbSet = _dbContext.Set<T>();
            _unitOfWork = unitOfWork;
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where);
        }

        public void Add(T entity, bool autoSave = true)
        {
            _dbSet.Add(entity);
            if (autoSave) _dbContext.SaveChanges();
        }

        public void Add(IEnumerable<T> entities, bool autoSave = true)
        {
            _dbSet.AddRange(entities);
            if (autoSave) _dbContext.SaveChanges();
        }

        public async Task AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> entities, bool autoSave = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public async Task<T> AddAsync(T entity, DbContext db, bool autoSave = true)
        {
            var savedEntity = db.Add(entity).Entity;

            if (autoSave)
                await db.SaveChangesAsync();
            return savedEntity;
        }

        public void Delete(T entity, bool autoSave = true)
        {
            if (entity is IFullAuditedEntity fullAudited)
            {
                fullAudited.IsDeleted = false;
                Update(entity, autoSave);
            }
            else
            {
                _dbSet.Remove(entity);

                if (autoSave) _dbContext.SaveChanges();
            }
        }

        public void Delete(object id, bool autoSave = true)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null) Delete(entity);
            }

            if (autoSave) _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities, bool autoSave = true)
        {
            bool isSoftDelete = IsSoftDelete(entities);

            if (isSoftDelete)
            {
                Update(entities, autoSave);
            }
            else
            {
                _dbSet.RemoveRange(entities);
                if (autoSave) _dbContext.SaveChanges();
            }
        }

        private static bool IsSoftDelete(IEnumerable<T> entities)
        {
            bool softDelete = false;
            foreach (var entity in entities)
            {
                if (entity is IFullAuditedEntity fullAuditedEntity)
                {
                    softDelete = true;
                    fullAuditedEntity.IsDeleted = true;
                }
                else
                    break;
            }

            return softDelete;
        }

        public async Task DeleteAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            bool isSoftDelete = IsSoftDelete(entities);
            if (isSoftDelete)
                await UpdateAsync(entities, autoSave);
            else
            {
                _dbSet.RemoveRange(entities);
                if (autoSave) await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(T entity, bool autoSave = true)
        {
            if (entity is IFullAuditedEntity fullAuditedEntity)
            {
                fullAuditedEntity.IsDeleted = true;
                await UpdateAsync(entity, autoSave);
            }
            else
            {
                _dbSet.Remove(entity);

                if (autoSave) await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(object id, bool autoSave = true)
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<T>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null) await DeleteAsync(entity);
            }

            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity, bool autoSave = true)
        {
            _dbSet.Update(entity);
            if (autoSave) _dbContext.SaveChanges();
        }

        public void Update(IEnumerable<T> entities, bool autoSave = true)
        {
            _dbSet.UpdateRange(entities);
            if (autoSave) _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity, bool autoSave = true)
        {
            _dbSet.Update(entity);
            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities, bool autoSave = true)
        {
            _dbSet.UpdateRange(entities);
            if (autoSave) await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

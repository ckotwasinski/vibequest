using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace VibeQuest.DataAccess.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);

        IQueryable<T> Get(Expression<Func<T, bool>> where);

        void Add(T entity, bool autoSave = true);
        void Add(IEnumerable<T> entities, bool autoSave = true);
        Task AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default(CancellationToken));
        Task AddAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(T entity, bool autoSave = true);
        void Delete(object id, bool autoSave = true);
        void Delete(IEnumerable<T> entities, bool autoSave = true);
        Task DeleteAsync(T entity, bool autoSave = true);
        Task DeleteAsync(object id, bool autoSave = true);
        Task DeleteAsync(IEnumerable<T> entities, bool autoSave = true);

        void Update(T entity, bool autoSave = true);
        void Update(IEnumerable<T> entities, bool autoSave = true);
        Task UpdateAsync(T entity, bool autoSave = true);
        Task UpdateAsync(IEnumerable<T> entities, bool autoSave = true);

        Task SaveChangeAsync();
    }
}

using System.Collections.Generic;
using System.Linq;
using Niqiu.Core.Domain;

namespace Niqiu.Core.Services
{
    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public interface IRepository<T> where T:BaseEntity
   {
       /// <summary>
       /// Gets the by id.
       /// </summary>
       /// <param name="id">The id.</param>
       /// <returns>`0.</returns>
       T GetById(object id);
       /// <summary>
       /// Inserts the specified entity.
       /// </summary>
       /// <param name="entity">The entity.</param>
       void Insert(T entity);
       /// <summary>
       /// Inserts the specified entities.
       /// </summary>
       /// <param name="entities">The entities.</param>
       void Insert(IEnumerable<T> entities);
       /// <summary>
       /// Updates the specified entity.
       /// </summary>
       /// <param name="entity">The entity.</param>
       void Update(T entity);
       /// <summary>
       /// Deletes the specified entity.
       /// </summary>
       /// <param name="entity">The entity.</param>
       void Delete(T entity);
       /// <summary>
       /// Gets the table.
       /// </summary>
       /// <value>The table.</value>
       IQueryable<T> Table { get; }
       /// <summary>
       /// Gets the tables no tracking.
       /// </summary>
       /// <value>The tables no tracking.</value>
       IQueryable<T> TableNoTracking { get; }
   }
}

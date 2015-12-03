using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Services;

namespace Portal.MVC.Services
{
   public class EfRepository<T>:IRepository<T> where T:BaseEntity
   {
       #region Fields

       private readonly IDbContext _context ;

       private IDbSet<T> _entities;

       #endregion
       public EfRepository(IDbContext context)
       {
           if (context == null)
           {
               throw new ArgumentNullException("context");
           }
           _context = context;
       }
       #region property

       protected virtual IDbSet<T> Entities
       {
           get
           {
               if (_entities == null)
               {
                   _entities = _context.Set<T>();
               }
               return _entities ?? (_entities = _context.Set<T>()); 
                  // _entities ?? _entities = db.Set<T>();
           }
       }

       /// <summary>
       /// Gets a table
       /// </summary>
       public virtual IQueryable<T> Table
       {
           get
           {
               return Entities;
           }
       }

       public IQueryable<T> TableNoTracking
       {
           get
           {
               return Entities.AsNoTracking();
               
           }
       }


       #endregion

     

       public virtual T GetById(object id)
       {
           return Entities.Find(id);
       }

       public void Insert(T entity)
       {
           try
           {
               if(entity==null) throw new ArgumentException("entity");
               Entities.Add(entity);
               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               GetException(dbEx);
           }
       }

       /// <summary>
       /// Insert entities
       /// </summary>
       /// <param name="entities">Entities</param>
       public virtual void Insert(IEnumerable<T> entities)
       {
           try
           {
               if (entities == null)
                   throw new ArgumentNullException("entities");

               foreach (var entity in entities)
                   Entities.Add(entity);

               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               GetException(dbEx);
           }
       }

       /// <summary>
       /// Update entity
       /// </summary>
       /// <param name="entity">Entity</param>
       public virtual void Update(T entity)
       {
           try
           {
               if (entity == null)
                   throw new ArgumentNullException("entity");
               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               GetException(dbEx);
           }
       }

       /// <summary>
       /// Delete entity
       /// </summary>
       /// <param name="entity">Entity</param>
       public virtual void Delete(T entity)
       {
           try
           {
               if (entity == null)
                   throw new ArgumentNullException("entity");

               Entities.Remove(entity);

               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               GetException(dbEx);
           }
       }

       /// <summary>
       /// Delete entities
       /// </summary>
       /// <param name="entities">Entities</param>
       public virtual void Delete(IEnumerable<T> entities)
       {
           try
           {
               if (entities == null)
                   throw new ArgumentNullException("entities");

               foreach (var entity in entities)
                   Entities.Remove(entity);
               _context.SaveChanges();
           }
           catch (DbEntityValidationException dbEx)
           {
               GetException(dbEx);
           }
       }

       private static void GetException(DbEntityValidationException dbEx)
       {
           var msg = string.Empty;

           foreach (var validationErrors in dbEx.EntityValidationErrors)
               foreach (var validationError in validationErrors.ValidationErrors)
                   msg += Environment.NewLine +
                          string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

           var fail = new Exception(msg, dbEx);
           //Debug.WriteLine(fail.Message, fail);
           throw fail;
       }
   }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using Niqiu.Core.Domain;
using Niqiu.Core.Domain.Catalog;
using Niqiu.Core.Domain.Common;
using Niqiu.Core.Domain.Discounts;
using Niqiu.Core.Domain.Orders;
using Niqiu.Core.Domain.Security;
using Niqiu.Core.Domain.User;
using Niqiu.Core.Mapping;
using Niqiu.Core.Mapping.Security;
using Niqiu.Core.Mapping.User;

namespace Portal.MVC.Models
{
    public class PortalDb : DbContext, IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<PermissionRecord> PermissionRecords { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<TierPrice> TierPrices { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountRequirement> DiscountRequirements { get; set; } 

        public PortalDb()
            : base("SqlConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PortalDb, Configuration<PortalDb>>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
     
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new PermissionRecordMap());
            //var typesToRegister =
            //    Assembly.GetExecutingAssembly().GetTypes().Where(type => !String.IsNullOrEmpty(type.Namespace)).ToList()
            //    .Where(type=>type.BaseType!=null&&type.BaseType.IsGenericType&&type.BaseType.GetGenericTypeDefinition()==typeof(PortalEntityTypeConfiguration<>)).ToList();
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configInstance = Activator.CreateInstance(type);
            //    modelBuilder.Configurations.Add(configInstance);
            //}
            base.OnModelCreating(modelBuilder);
        }


        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }
            return result;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }
    }

    internal sealed class Configuration<TContext> : DbMigrationsConfiguration<TContext> where TContext : DbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
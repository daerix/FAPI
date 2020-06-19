using ApiLibrary.Core.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibrary.Core.Entity
{
    public abstract class BaseDbContext : DbContext
    {
        public BaseDbContext() : base()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTracking();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTracking();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTracking()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var item in entries)
            {
                if (item.State == EntityState.Modified)
                {
                    var properties = item.Entity.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        if(prop.GetCustomAttribute<NoModifiedAttribute>() != null)
                        {
                            item.Property(prop.Name).IsModified = false;
                        }
                    }

                    /*item.Property("CreatedAt").IsModified = false;
                    item.Property("ID").IsModified = false;
                    item.Property("DeletedAt").IsModified = false;*/
                   ((BaseEntity)item.Entity).UpdatedAt = DateTime.Now;
                }

                if (item.State == EntityState.Deleted)
                {
                    ((BaseEntity)item.Entity).DeletedAt = DateTime.Now;
                    //item.Property("DeletedAt").IsModified = true;
                    item.State = EntityState.Modified;
                }

                if (item.State == EntityState.Added)
                {
                    ((BaseEntity)item.Entity).CreatedAt = DateTime.Now;
                    //item.Property("CreatedAt").IsModified = true;
                }
               
            }
        }

    }
}

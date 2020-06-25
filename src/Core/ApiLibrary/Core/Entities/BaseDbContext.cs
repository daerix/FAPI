using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiLibrary.Core.Attributes;


namespace ApiLibrary.Core.Entities
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTracking();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTracking()
        {
            var entries =  ChangeTracker.Entries().Where(x => 
            x.Entity is BaseEntity 
            && (x.State == EntityState.Added 
            || x.State == EntityState.Modified 
            || x.State == EntityState.Deleted));

            foreach (var item in entries)
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        foreach (var prop in item.Entity.GetType().GetProperties())
                        {
                            if (prop.GetCustomAttribute(typeof(NoModifiedAttribute)) != null)
                            {
                                item.Property(prop.Name).IsModified = false;
                            }
                        }
                        break;

                    case EntityState.Added:
                        ((BaseEntity)item.Entity).CreatedAt = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        ((BaseEntity)item.Entity).DeletedAt = DateTime.Now;
                        item.State = EntityState.Modified;
                        break;
                }
                ((BaseEntity)item.Entity).UpdatedAt = DateTime.Now;
            }
        }
    }
}

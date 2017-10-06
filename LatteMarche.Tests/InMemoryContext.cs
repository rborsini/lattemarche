using LatteMarche.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests
{
    public class InMemoryContext : IContext
    {
        public int SaveChangesCount { get; private set; }

        public int SaveChanges()
        {
            this.SaveChangesCount++;
            return 1;
        }

        public DbSet<TEntity> Set<TEntity, TPrimaryKey>()
            where TEntity : Entity<TPrimaryKey>
        {
            return new InMemoryDbSet<TEntity, TPrimaryKey>();
        }

        public void SetModified(object entity)
        {

        }

    }
}

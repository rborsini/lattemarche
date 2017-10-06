using LatteMarche.Core;
using LatteMarche.Core.Models;
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

        public DbSet<AllevamentoXGiro> AllevamentiXGiro { get; set; }

        public InMemoryContext()
        {
            this.AllevamentiXGiro = new InMemoryDbSet<AllevamentoXGiro>();
        }

        public int SaveChanges()
        {
            this.SaveChangesCount++;
            return 1;
        }

        public DbSet<TEntity> Set<TEntity>()
              where TEntity : class
        {
            return new InMemoryDbSet<TEntity>();
        }

        public void SetModified(object entity)
        {

        }

    }
}

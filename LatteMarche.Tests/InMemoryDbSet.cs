using LatteMarche.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests
{
    public class InMemoryDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>
        where TEntity : class
    {

        ObservableCollection<TEntity> data;
        IQueryable query;

        public InMemoryDbSet()
        {
            this.data = new ObservableCollection<TEntity>();
            this.query = this.data.AsQueryable();
        }

        public override TEntity Find(params object[] keyValues)
        {
            string json = JsonConvert.SerializeObject(this.data);

            JArray jarr = JArray.Parse(json);
            foreach (JObject content in jarr.Children<JObject>())
            {
                foreach (JProperty prop in content.Properties())
                {
                    if (prop.Name == "Id" && (string)prop.Value == keyValues[0].ToString())
                        return content.ToObject<TEntity>();
                                                           
                }
            }


            return null;
            
        }

        public override TEntity Add(TEntity item)
        {
            this.data.Add(item);
            return item;
        }

        public override TEntity Remove(TEntity item)
        {
            this.data.Remove(item);
            return item;
        }

        public override TEntity Attach(TEntity item)
        {
            Entity<object> entity = item as Entity<object>;
            switch (entity.ObjectState)
            {
                case ObjectState.Modified:
                    data.Remove(item);
                    data.Add(item);
                    break;

                case ObjectState.Deleted:
                    data.Remove(item);
                    break;

                case ObjectState.Unchanged:
                case ObjectState.Added:
                    data.Add(item);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return item;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return this.data; }
        }

        Type IQueryable.ElementType
        {
            get { return this.query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return this.query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(this.query.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        //IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        //{
        //    return new DbAsyncEnumerator<TEntity>(this.data.GetEnumerator());
        //} 

    }
}

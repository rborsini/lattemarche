using System;
using System.Collections.Generic;
using System.Text;

namespace LatteMarche.Service.Data
{
    public interface IRepository<T, TKey>
        where T : class
    {
        List<T> GetAll();

        T GetById(TKey key);

        void Add(T record);

        void Update(T record);

        void Delete(T record);

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IDataStore<TEntity, TPrimaryKey>
    {
        Task<bool> AddRangeItemAsync(IEnumerable<TEntity> items);
        Task<bool> AddItemAsync(TEntity item);
        Task<bool> UpdateItemAsync(TEntity item);
        Task<bool> DeleteItemAsync(TPrimaryKey id);
        Task<bool> DeleteAllItemsAsync();
        Task<TEntity> GetItemAsync(TPrimaryKey id);
        Task<IEnumerable<TEntity>> GetItemsAsync(bool forceRefresh = false);
    }
}

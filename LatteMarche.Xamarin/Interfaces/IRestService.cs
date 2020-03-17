using LatteMarche.Xamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IRestService
    {
        Task<bool> GetToken(string username, string password);

        Task<List<Allevamento>> GetAllevamenti();

    }
}
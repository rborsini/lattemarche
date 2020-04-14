using LatteMarche.Xamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Interfaces
{
    public interface IRestService
    {
        Task<bool> GetToken(string username, string password);

        Task<List<Acquirente>> GetAcquirenti();

        Task<List<Allevamento>> GetAllevamenti();

        Task<List<AutoCisterna>> GetAutoCisterne();

        Task<List<Destinatario>> GetDestinatari();

        Task<List<Giro>> GetGiri();

        Task<List<GiroItem>> GetGiro(int idGiro);

        Task<List<TipoLatte>> GetTipiLatte();

        Task<List<Trasportatore>> GetTrasportatori();

    }
}
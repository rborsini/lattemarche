using LatteMarche.Xamarin.Rest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Rest.Interfaces
{
    public interface IRestService
    {

        Task<DispositivoDto> Register(DispositivoDto dto);

        Task<DownloadDto> DownloadDb(string imei);

        Task<bool> UploadPrelievi(UploadDto dto);

        Task<TrasbordoDto> UploadTrasbordo(TrasbordoDto trasbordo);

        Task<List<TrasbordoDto>> DownloadTrasbordi(string imei);

        Task<bool> ChiudiTrasbordo(long id);
    }
}
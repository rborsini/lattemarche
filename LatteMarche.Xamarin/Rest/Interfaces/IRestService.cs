using LatteMarche.Xamarin.Rest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LatteMarche.Xamarin.Rest.Interfaces
{
    public interface IRestService
    {

        Task<DispositivoDto> Register(DispositivoDto dto);

        Task<DownloadDto> Download(string imei);


    }
}
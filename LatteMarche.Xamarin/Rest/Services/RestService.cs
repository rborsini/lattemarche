using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.CSharp;
using LatteMarche.Xamarin.Interfaces;
using System.Threading;
using System.Linq;
using LatteMarche.Xamarin.Rest.Interfaces;
using LatteMarche.Xamarin.Rest.Dtos;
using LatteMarche.Xamarin.Db.Interfaces;

namespace LatteMarche.Xamarin.Rest.Services
{
    public class RestService : IRestService
    {

        #region Fields

        private IAmbientiService ambientiService = DependencyService.Get<IAmbientiService>();

        private string endpoint
        {
            get
            {
                var ambiente = this.ambientiService.GetDefault();
                return ambiente != null ? ambiente.Url : "";
            }
        }

        #endregion

        #region Methods

        public async Task<DispositivoDto> Register(DispositivoDto dto)
        {
            var url = $"{this.endpoint}/api/Mobile/Register";

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var json = JsonConvert.SerializeObject(dto);

            request.AddParameter("application/json,text/plain", json, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            var responseDto = JsonConvert.DeserializeObject<DispositivoDto>(response.Content);

            return responseDto;

        }

        public async Task<DownloadDto> DownloadDb(string imei)
        {
            var url = $"{this.endpoint}/api/Mobile/Download?imei={imei}";
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            var dto = JsonConvert.DeserializeObject<DownloadDto>(response.Content);

            return dto;
        }

        public async Task<bool> UploadPrelievi(UploadDto dto)
        {
            var url = $"{this.endpoint}/api/Mobile/Upload";

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var json = JsonConvert.SerializeObject(dto);

            request.AddParameter("application/json,text/plain", json, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<TrasbordoDto> UploadTrasbordo(TrasbordoDto dto)
        {
            var url = $"{this.endpoint}/api/Trasbordi/Push";

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var json = JsonConvert.SerializeObject(dto);

            request.AddParameter("application/json,text/plain", json, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<TrasbordoDto>(response.Content);
        }

        public async Task<List<TrasbordoDto>> DownloadTrasbordi(string imei)
        {
            var url = $"{this.endpoint}/api/Trasbordi/Pull?imei={imei}";
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if (!String.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<List<TrasbordoDto>>(response.Content);
            else
                return new List<TrasbordoDto>();
        }


        public async Task<bool> ChiudiTrasbordo(long id)
        {
            var url = $"{this.endpoint}/api/Trasbordi/Close?id={id}";

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json,text/plain", "", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);

            return true;
        }

        #endregion

    }
}

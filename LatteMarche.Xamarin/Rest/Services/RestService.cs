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

namespace LatteMarche.Xamarin.Rest.Services
{
    public class RestService : IRestService
    {
        private const string API_ENDPOINT = "http://robertoborsini.myqnapcloud.com:81";

        public async Task<DispositivoDto> Register(DispositivoDto dto)
        {
            var url = $"{API_ENDPOINT}/api/Mobile/Register";

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

        public async Task<DownloadDto> Download(string imei)
        {
            var client = new RestClient($"{API_ENDPOINT}/api/Mobile/Download?imei={imei}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            var dto = JsonConvert.DeserializeObject<DownloadDto>(response.Content);

            return dto;
        }


    }
}

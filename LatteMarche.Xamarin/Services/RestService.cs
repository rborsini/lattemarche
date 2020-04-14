using LatteMarche.Xamarin.Models;
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

namespace LatteMarche.Xamarin.Services
{
    public class RestService : IRestService
    {
        private const string API_ENDPOINT = "http://192.168.0.73:81";

        private string Token
        {
            get { return Application.Current.Properties["token"].ToString(); }
            set { Application.Current.Properties["token"] = value; }
        }

        public async Task<bool> GetToken(string username, string password)
        {
            try
            {
                var client = new RestClient($"{API_ENDPOINT}/Token");

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                IRestResponse response = client.Execute(request);


                JObject responseObj = JObject.Parse(response.Content);
                this.Token = responseObj["access_token"].Value<string>();

                return await Task.FromResult<bool>(true);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return await Task.FromResult<bool>(false);
            }
        }

        public async Task<List<Acquirente>> GetAcquirenti()
        {
            return await GetRecords<Acquirente>("Acquirenti");
        }

        public async Task<List<Allevamento>> GetAllevamenti()
        {
            return await GetRecords<Allevamento>("Allevamenti");
        }

        public async Task<List<AutoCisterna>> GetAutoCisterne()
        {
            return await GetRecords<AutoCisterna>("autocisterne");
        }

        public async Task<List<Destinatario>> GetDestinatari()
        {
            return await GetRecords<Destinatario>("Destinatari");
        }

        public async Task<List<Giro>> GetGiri()
        {
            return await GetRecords<Giro>("Giri");
        }

        public async Task<List<TipoLatte>> GetTipiLatte()
        {
            return await GetRecords<TipoLatte>("tipiLatte");
        }

        public async Task<List<Trasportatore>> GetTrasportatori()
        {
            return await GetRecords<Trasportatore>("trasportatori");
        }

        public async Task<List<GiroItem>> GetGiro(int idGiro)
        {
            var client = new RestClient($"{API_ENDPOINT}/api/giri/details?id={idGiro}");
            client.Timeout = -1;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token, "Bearer");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            var itemsJson = JObject.Parse(response.Content).SelectToken("Items").ToString();
            var list = JsonConvert.DeserializeObject<List<GiroItem>>(itemsJson);

            list = list.Where(item => item.Priorita.HasValue).ToList();

            return await Task.FromResult<List<GiroItem>>(list);
        }

        private async Task<List<TDto>> GetRecords<TDto>(string dtoName)
            where TDto : class
        {
            var client = new RestClient($"{API_ENDPOINT}/api/{dtoName}");
            client.Timeout = -1;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token, "Bearer");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            var list = JsonConvert.DeserializeObject<List<TDto>>(response.Content);

            return await Task.FromResult<List<TDto>>(list);
        }


    }
}

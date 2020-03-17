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

        public async Task<List<Allevamento>> GetAllevamenti()
        {
            var client = new RestClient($"{API_ENDPOINT}/api/Allevamenti");
            client.Timeout = -1;
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.Token, "Bearer");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            var list = JsonConvert.DeserializeObject<List<Allevamento>>(response.Content);

            return await Task.FromResult<List<Allevamento>>(list);
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
    }
}

using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Sitra.Interfaces;
using LatteMarche.Application.Sitra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace LatteMarche.Application.Sitra.Services
{
    public class SitraService : ISitraService
    {
        public List<LottoDto> InvioLotti(List<LottoDto> lotti)
        {
            // Conversione dal modello LatteMarche a modello Sitra



            #region Recupero token

            dynamic tokenResult = Token();
            string accessToken = "";// tokenResult.access_token;

            #endregion

            #region Invio lotti
            SitraDto root = MakeRoot();
            string codiceLotto = SendLotto(accessToken, root);

            Console.WriteLine($"codice lotto {codiceLotto}");

            Console.ReadKey();
            #endregion

            // Aggiornamento campi lotti (Es. Inviato, Errore, Messaggio, Timestamp, CodiceSitra)

            return lotti; //TODO: sistemare questa parte

        }

        private string SendLotto(string accessToken, SitraDto root)
        {
            var client = new RestClient("http://84.38.48.58/SitraSmartTest/WebApi/api/lotti/");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(root), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Content;
            else
                return String.Empty;
        }

        private static dynamic Token()
        {
            string URI = "http://84.38.48.58/SitraSmartTest/WebApi/Token"; //TODO: portare rui in configuration file

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("grant_type", "password");
                parameters.Add("username", "operatore2");
                parameters.Add("password", "operatore2");

                byte[] responseBytes = wc.UploadValues(URI, "POST", parameters);
                string responseBody = Encoding.UTF8.GetString(responseBytes);

                return JsonConvert.DeserializeObject(responseBody);
            }
        }

        private static SitraDto MakeRoot()
        {
            SitraDto root = new SitraDto();

            root.Lotto.CodiceLotto = Guid.NewGuid().ToString();
            root.Lotto.DataProduzione = DateTime.Today.ToString("dd/MM/yyyy");
            root.Lotto.Quantita = DateTime.Now.Second;
            root.Lotto.IdUnitaMisura = 3; // litri
            root.Lotto.Referenza = "Referenza";
            root.Lotto.CodOperatore = "OPER2";
            root.Lotto.IdProdotto = 28;
            root.Lotto.CUAA = 12345678900;

            // Data scadenza
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 173,
                NomeAttributo = "Data di scadenza",
                Obbligatorio = true,
                TipoDiDato = "data",
                ValoreAttributo = DateTime.Today.AddDays(7).ToString("dd/MM/yyyy")
            });

            // Modalità conservazione
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 174,
                NomeAttributo = "Modalità di conservazione",
                Obbligatorio = true,
                TipoDiDato = "testo",
                ValoreAttributo = "Frigo"
            });

            // Data di confezionamento
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 204,
                NomeAttributo = "Data di confezionamento",
                Obbligatorio = true,
                TipoDiDato = "testo",
                ValoreAttributo = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy")
            });

            // Tenore materia grassa
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 206,
                NomeAttributo = "Tenore di Materia Grassa (% p/p)",
                Obbligatorio = true,
                TipoDiDato = "numero",
                ValoreAttributo = DateTime.Now.Minute.ToString()
            });

            // Tenore materia proteica
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 207,
                NomeAttributo = "Tenore di materia Proteica (g/litro)",
                Obbligatorio = true,
                TipoDiDato = "numero",
                ValoreAttributo = DateTime.Now.Millisecond.ToString()
            });

            // Tipologia caglio
            root.AttributiList.Add(new Attributo()
            {
                IdAttributo = 159,
                NomeAttributo = "Tipologia di caglio",
                Obbligatorio = false,
                TipoDiDato = "testo",
                ValoreAttributo = "Caglio"
            });

            // Temperatura stoccaggio
            root.AttributiList.Add(new Attributo()
            {
                IdAttributo = 166,
                NomeAttributo = "Temperatura di stoccaggio latte dopo mungitura (°C)",
                Obbligatorio = true,
                TipoDiDato = "numero",
                ValoreAttributo = (-DateTime.Now.Minute).ToString()
            });

            // Temperatura latte all'arrivo
            root.AttributiList.Add(new Attributo()
            {
                IdAttributo = 167,
                NomeAttributo = "Temperatura latte all'arrivo presso lo stabilimento di trasformazione (°C)",
                Obbligatorio = false,
                TipoDiDato = "numero",
                ValoreAttributo = (-DateTime.Now.Hour).ToString()
            });

            return root;
        }
    }
}
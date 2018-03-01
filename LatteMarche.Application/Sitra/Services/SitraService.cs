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
using System.Configuration;


namespace LatteMarche.Application.Sitra.Services
{
    public class SitraService : ISitraService
    {
        #region Proprietis
        private static string sitraUrl = ConfigurationManager.AppSettings["Sitra.ServiceUri"];
        private static string username = ConfigurationManager.AppSettings["username"];
        private static string password = ConfigurationManager.AppSettings["password"];
        private static int CUAA { get { return Convert.ToInt32(ConfigurationManager.AppSettings["CUAA"]); } }
        private static int IdProdotto { get { return Convert.ToInt32(ConfigurationManager.AppSettings["idProdotto"]); } }
        private static string codOperatore = ConfigurationManager.AppSettings["codOperatore"];
        private static string referenza = ConfigurationManager.AppSettings["referenza"];
        private static int IdUnitaMisura { get { return Convert.ToInt32(ConfigurationManager.AppSettings["IdUnitaMisura"]); } }
        private static string alimentazione = ConfigurationManager.AppSettings["alimentazione"];
        private static Double MassaGrassa { get { return Convert.ToDouble(ConfigurationManager.AppSettings["massa_grassa"]); } }
        private static Double MassaProteica { get { return Convert.ToDouble(ConfigurationManager.AppSettings["massa_proteica"]); } }
        #endregion

        public List<LottoDto> InvioLotti(List<LottoDto> lotti)
        {
            // Conversione dal modello LatteMarche a modello Sitra

            dynamic tokenResult = Token();
            string accessToken = tokenResult.access_token;

            //invio lotti
            foreach (LottoDto lotto in lotti)
            {
                SitraDto root = MakeRoot(lotto);
                try
                {
                    lotto.CodiceSitra = SendLotto(accessToken, root);
                    lotto.Inviato = true;
                    //TODO: messaggio
                    //lotto.Messaggio = "mex"
                }
                catch
                {
                    lotto.Inviato = false;
                    //TODO: inserire eventuale errore
                    //lotto.Errore = "qualcosa"
                }
                lotto.TimeStamp = DateTime.Now;
            }

            // Aggiornamento campi lotti (Es. Inviato, Errore, Messaggio, Timestamp, CodiceSitra)

            return lotti; //TODO: sistemare questa parte

        }

        private string SendLotto(string accessToken, SitraDto root)
        {
            var client = new RestClient(sitraUrl);
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
            string URI = ConfigurationManager.AppSettings["Sitra.ServiceUri"];//sitraUrl;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("grant_type", "password");
                parameters.Add("username", username);
                parameters.Add("password", password);

                byte[] responseBytes = wc.UploadValues(URI, "POST", parameters);
                string responseBody = Encoding.UTF8.GetString(responseBytes);

                return JsonConvert.DeserializeObject(responseBody);
            }
        }

        private static SitraDto MakeRoot(LottoDto lotto)
        {


            SitraDto root = new SitraDto();

            root.Lotto.CodiceLotto = lotto.Codice;
            root.Lotto.DataProduzione = lotto.DataConsegna.ToString("dd/MM/yyyy"); //ATTENZIONE: è giusto?
            root.Lotto.Quantita = Convert.ToInt32(lotto.Quantita);
            root.Lotto.IdUnitaMisura = IdUnitaMisura; // TODO: Ricordare fattore di conversione per il passaggio dei dati
            root.Lotto.Referenza = referenza;
            root.Lotto.CodOperatore = codOperatore;
            root.Lotto.IdProdotto = IdProdotto;
            root.Lotto.CUAA = CUAA;

            // Data ultima mungitura
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 156,
                NomeAttributo = "Data ultima mungitura",
                Obbligatorio = true,
                TipoDiDato = "data",
                ValoreAttributo = lotto.DataUltimaMungitura.ToString("dd/MM/yyyy")
            });

            // Tipologia di alimentazione animale prevalente
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 164,
                NomeAttributo = "Tipologia di alimentazione animale prevalente",
                Obbligatorio = true,
                TipoDiDato = "testo",
                ValoreAttributo = alimentazione
            });

            // Ora Ultima Mungitura
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 203,
                NomeAttributo = "Ora Ultima Mungitura",
                Obbligatorio = true,
                TipoDiDato = "testo",
                ValoreAttributo = lotto.DataUltimaMungitura.ToString("HH:mm")
            });

            // Tenore materia grassa
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 206,
                NomeAttributo = "Tenore di Materia Grassa (% p/p)",
                Obbligatorio = true,
                TipoDiDato = "numero",
                ValoreAttributo = Convert.ToString(MassaGrassa)
            });

            // Tenore materia proteica
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 207,
                NomeAttributo = "Tenore di materia Proteica (g/litro)",
                Obbligatorio = true,
                TipoDiDato = "numero",
                ValoreAttributo = Convert.ToString(MassaProteica)
            });

            return root;
        }

    }
}
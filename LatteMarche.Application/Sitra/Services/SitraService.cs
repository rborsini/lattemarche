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
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using LatteMarche.Application.Lotti.Interfaces;
using System.Threading;
using LatteMarche.Application.Logs.Interfaces;

namespace LatteMarche.Application.Sitra.Services
{
    public class SitraService : ISitraService
    {
        #region  Fields

        private IAllevamentiService allevamentiService;
        private ILottiService lottiService;
        private IPrelieviLatteService prelieviLatteService;
        private ILogsService logsService;

        #endregion

        #region Proprieties

        private static string sitraUrl = ConfigurationManager.AppSettings["Sitra.ServiceUri"];

        private static string username = ConfigurationManager.AppSettings["username"];
        private static string password = ConfigurationManager.AppSettings["password"];
        private static string codOperatore = ConfigurationManager.AppSettings["codOperatore"];

        private static int idLatteCrudo { get { return Convert.ToInt32(ConfigurationManager.AppSettings["idLatteCrudo"]); } }
        private static int idLatteCrudoStoccatoQM { get { return Convert.ToInt32(ConfigurationManager.AppSettings["idLatteCrudoStoccatoQM"]); } }

        private static string CUAA { get { return ConfigurationManager.AppSettings["CUAA"]; } }
        private static int IdUnitaMisura { get { return Convert.ToInt32(ConfigurationManager.AppSettings["IdUnitaMisura"]); } }

        #endregion

        #region  Constructors

        public SitraService(IAllevamentiService allevamentiService, IPrelieviLatteService prelieviLatteService, ILottiService lottiService, ILogsService logsService)
        {
            this.allevamentiService = allevamentiService;
            this.prelieviLatteService = prelieviLatteService;
            this.lottiService = lottiService;
            this.logsService = logsService;
        }

        #endregion

        #region  Methods


        /// <summary>
        /// Invio singoli prelievi (Aziende allevatori)
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        public List<PrelievoLatteDto> InvioPrelievi(List<PrelievoLatteDto> prelievi)
        {
            List<PrelievoLatteDto> prelieviInviati = new List<PrelievoLatteDto>();

            // autenticazione
            dynamic tokenResult = Token();
            string accessToken = tokenResult.access_token;

            //this.LogDebug($"accessToken [{accessToken}]");

            // recupero allevamenti per cui si deve effettuare l'invio al sitra
            Dictionary<int, AllevamentoDto> allevamentiSitra = this.allevamentiService.GetAllevamentiSitra().ToDictionary(k => k.Id, k => k);

            // invio singoli prelievi 
            foreach (var prelievo in prelievi.Where(p => allevamentiSitra.Keys.Contains(p.IdAllevamento.Value)))
            {
                // se non è già stato inviato
                if(!Sent(prelievo))
                {
                    // testata del documento da inviare
                    SitraDto root = MakeRoot(prelievo, allevamentiSitra[prelievo.IdAllevamento.Value]);

                    try
                    {
                        // invio lotto e aggiornamento codice sitra
                        prelievo.CodiceSitra = InserimentoLotto(accessToken, root);
                        //this.LogDebug($"Prelievo inviato correttamente [{prelievo.Id}]");
                    }
                    catch(Exception exc)
                    {
                        //this.LogDebug($"Errore invio SITRA [{exc.Message}]");
                        prelievo.CodiceSitra = "ERRORE_INVIO";
                    }
                    finally
                    {
                        prelieviInviati.Add(this.prelieviLatteService.Update(prelievo));
                    }
                }
                else
                {
                    //this.LogDebug($"prelievo già inviato in passato [{prelievo.Id}]");
                }

            }

            return prelieviInviati;
        }

        /// <summary>
        /// Verifica condizione inviato sitra
        /// </summary>
        /// <param name="prelievo"></param>
        /// <returns></returns>
        private bool Sent(PrelievoLatteDto prelievo)
        {
            return !String.IsNullOrEmpty(prelievo.CodiceSitra) && prelievo.CodiceSitra != "ERRORE_INVIO";
        }

        /// <summary>
        /// Invio lotti (Latte Marche)
        /// </summary>
        /// <param name="lotti"></param>
        /// <returns></returns>
        public List<LottoDto> InvioLotti(List<LottoDto> lotti)
        {
            // Conversione dal modello LatteMarche a modello Sitra
            dynamic tokenResult = Token();
            string accessToken = tokenResult.access_token;

            foreach (LottoDto lotto in lotti)
            {

                SitraDto root = MakeRoot(lotto);
                try
                {
                    lotto.CodiceSitra = InserimentoLotto(accessToken, root);
                    lotto.Inviato = true;                    
                    lotto.Messaggio = $"Depositati {(int)lotto.Quantita} kg di latte";

                    // aggancio lotti padre
                    foreach (var prelievoPadre in lotto.PrelieviPadre)
                    {
                        this.LogDebug($"InvioLotti {JsonConvert.SerializeObject(prelievoPadre)}");

                        InserimentoLottoPadre(accessToken, new LottoPadreDto()
                        {
                            IdLotto = Convert.ToInt32(lotto.CodiceSitra),
                            IdLottoPadre = Convert.ToInt32(prelievoPadre.CodiceSitra),
                            Quantita = prelievoPadre.Quantita.HasValue ? prelievoPadre.Quantita.Value : 0,
                            IdUnitaMisura = IdUnitaMisura
                        });
                    }
                }
                catch(Exception exc)
                {
                    lotto.Inviato = false;
                    lotto.Messaggio = "Dati lotto non inviati";
                    lotto.Errore = true;
                }
                lotto.TimeStamp = DateTime.Now;
            }

            return lotti; 

        }

        /// <summary>
        /// Chiamata REST per l'inserimento del lotto
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private string InserimentoLotto(string accessToken, SitraDto root)
        {
            var lotto = this.lottiService.GetByCodiceLotto(root.Lotto.CodiceLotto);

            if (Sent(lotto))
                return lotto.CodiceSitra;

            var client = new RestClient($"{sitraUrl}/api/lotti");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(root), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //this.LogDebug($"Response lotto {root.Lotto.CodiceLotto} [{JsonConvert.SerializeObject(response)}]");

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Content.Replace('"', ' ').Trim();
            else
                return String.Empty;
        }

        /// <summary>
        /// Verifica se lotto già inviato
        /// </summary>
        /// <param name="lotto"></param>
        /// <returns></returns>
        private bool Sent(LottoDto lotto)
        {
            return lotto != null && !String.IsNullOrEmpty(lotto.CodiceSitra) && lotto.Inviato;
        }

        /// <summary>
        /// Verifica set lotto padre già inviato
        /// </summary>
        /// <param name="lottoPadre"></param>
        /// <returns></returns>
        private bool Sent(string accessToken, LottoPadreDto lottoPadre)
        {
            var client = new RestClient($"{sitraUrl}/api/lottiPadre/{lottoPadre.IdLotto}");
            var request = new RestRequest(Method.GET);
            
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            IRestResponse response = client.Execute(request);

            List<LottoPadreDto> lottiPadre = JsonConvert.DeserializeObject<List<LottoPadreDto>>(response.Content);

            return lottiPadre != null && lottiPadre.Count > 0 && lottiPadre.Select(l => l.IdLottoPadre).Contains(lottoPadre.IdLottoPadre);

        }

        /// <summary>
        /// Chiamata REST per l'inserimento del lotto padre
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private string InserimentoLottoPadre(string accessToken, LottoPadreDto lottoPadre)
        {
            this.LogDebug($"InserimentoLottoPadre {JsonConvert.SerializeObject(lottoPadre)}");

            if (Sent(accessToken, lottoPadre))
                return "";

            var client = new RestClient($"{sitraUrl}/api/lottiPadre");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(lottoPadre), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            this.LogDebug($"Response lotto padre {JsonConvert.SerializeObject(lottoPadre)} [{JsonConvert.SerializeObject(response)}]");

            if (response.StatusCode == HttpStatusCode.NoContent)
                return response.Content.Replace('"', ' ').Trim();
            else
                return String.Empty;
        }

        

        /// <summary>
        /// Autenticazione e recupero del token di accesso
        /// </summary>
        /// <returns></returns>
        private dynamic Token()
        {
            string URI = $"{ConfigurationManager.AppSettings["Sitra.ServiceUri"]}/Token";

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

        /// <summary>
        /// Costruzione struttura dati da inviare
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="cuaa"></param>
        /// <returns></returns>
        private SitraDto MakeRoot(PrelievoLatteDto prelievo, AllevamentoDto allevamento)
        {
            SitraDto root = new SitraDto();

            root.Lotto.CodiceLotto = prelievo.DataConsegna.Value.ToString("ddMMyy");
            root.Lotto.DataProduzione = prelievo.DataConsegna.Value.ToString("dd/MM/yyyy");
            //root.Lotto.Quantita = Convert.ToInt32(prelievo.Quantita * this.allevamentiService.GetFattoreConversione(allevamento.Id));
            root.Lotto.Quantita = prelievo.Quantita.HasValue ? prelievo.Quantita.Value : 0;
            root.Lotto.IdUnitaMisura = IdUnitaMisura; 
            root.Lotto.CodOperatore = codOperatore;
            root.Lotto.IdProdotto = idLatteCrudo;
            root.Lotto.CUAA = allevamento.CUAA;

            // Data ultima mungitura
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 122,
                NomeAttributo = "Data ultima mungitura",
                Obbligatorio = true,
                TipoDiDato = "data",
                ValoreAttributo = prelievo.DataUltimaMungitura.HasValue ? prelievo.DataUltimaMungitura.Value.ToString("dd/MM/yyyy") : String.Empty
            });

            // Ora Ultima Mungitura
            root.AttributiBaseList.Add(new Attributo()
            {
                IdAttributo = 242,
                NomeAttributo = "Ora Ultima Mungitura",
                Obbligatorio = true,
                TipoDiDato = "testo",
                ValoreAttributo = prelievo.DataUltimaMungitura.HasValue ? prelievo.DataUltimaMungitura.Value.ToString("HH:mm") : String.Empty
            });

            return root;
        }

        /// <summary>
        /// Costruzione struttura dati da inviare
        /// </summary>
        /// <param name="lotto"></param>
        /// <returns></returns>
        private SitraDto MakeRoot(LottoDto lotto)
        {
            SitraDto root = new SitraDto();

            root.Lotto.CodiceLotto = lotto.Codice;
            root.Lotto.DataProduzione = lotto.DataConsegna.ToString("dd/MM/yyyy"); 
            root.Lotto.Quantita = Convert.ToInt32(lotto.Quantita);
            root.Lotto.IdUnitaMisura = IdUnitaMisura; 
            root.Lotto.CodOperatore = codOperatore;
            root.Lotto.IdProdotto = idLatteCrudoStoccatoQM;
            root.Lotto.CUAA = CUAA;

            return root;
        }

        private void LogDebug(string message)
        {
            this.logsService.Create(new Application.Logs.Dtos.LogRecordDto()
            {
                Date = DateTime.Now,
                Level = "DEBUG",
                Logger = "api",
                Message = message
            });
        }

        #endregion

    }
}
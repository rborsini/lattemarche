using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace LatteMarche.Service.Jobs
{
    public class SynchJob : BaseJob
    {
        #region Properties

        //private static string baseUrl = "http://localhost:50364/";
        private static string baseUrl = "http://lattemarche-dev.azurewebsites.net";

        #endregion

        #region Constructors

        public SynchJob()
            : base() { }

        #endregion

        #region Methods
        static void Synchronizator()
        {

            // get last timestamp tabella synch
            string connectionString = ConfigurationManager.ConnectionStrings["DbLatteMarcheContext"].ConnectionString;

            TableSynchOperations synchTable = new TableSynchOperations(connectionString);

            DateTime lastTimeStamp = synchTable.GetDateLastSynch();
            System.Console.WriteLine($"Last Timestamp {lastTimeStamp.ToString()}\n");

            // pull servizi azure

            List<PrelievoLatteDto> prelievi = PullRequest(lastTimeStamp);
            System.Console.WriteLine($"Prelievi Count {prelievi.Count()}\n");
            synchTable.UpdateSyncTable(OperationTypeEnum.Pull);

            // insert or update prelievi da servizi azure

            InsertOrUpdate(prelievi, connectionString);

            // select ultimi n prelievi locali

            List<PrelievoLatteDto> prelieviPush = SelectLastPrelievi(connectionString);

            // push servizi azure


            PushRecords(prelieviPush);

            // aggiornamento tabella synch (nuovo push)

            synchTable.UpdateSyncTable(OperationTypeEnum.Push);




            System.Console.ReadKey();
        }



        public override void Execute()
        {
            this.log.Debug("Debug synch job");
            this.log.Info("Info synch job");

            System.Threading.Thread.Sleep(6000);


        }

        private static void PushRecords(List<PrelievoLatteDto> prelievi)
        {
            string page = baseUrl + "/Api/PrelieviLatte/push";
            var range = Convert.ToInt32(ConfigurationManager.AppSettings["range_synch"]);

            System.Console.Write("Package ");


            for (int i = 0; i <= prelievi.Count + range; i = i + range)
            {
                string prelievoJson = JsonConvert.SerializeObject(prelievi.Skip(i).Take(range));
                //System.Console.WriteLine(" ");
                //System.Console.WriteLine(prelievoJson);
                RestRequestPost(page, prelievoJson);

                System.Console.Write($"{1 + (i / range)}, ");


            }
            System.Console.Write($"sended\nPrelievi sended n:{prelievi.Count}\n");
        }

        private static List<PrelievoLatteDto> SelectLastPrelievi(string connectionString)
        {
            TablePrelieviOperations select = new TablePrelieviOperations (connectionString);
            List<PrelievoLatteDto> prelieviPush = select.Select_Prelievi();
            return prelieviPush;
        }


        private static void InsertOrUpdate(List<PrelievoLatteDto> prelievi, string connectionString)
        {
            int add = 0, upd = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            TablePrelieviOperations operation = new TablePrelieviOperations(connectionString);

            foreach (PrelievoLatteDto prelievo in prelievi)
            {
                switch (prelievo.LastOperation)
                {
                    case OperationEnum.Added: operation.InsertRecord(prelievo, connection); add++; break;
                    case OperationEnum.Updated: operation.UpdateRecord(prelievo, connection); upd++; break;
                    default: break;
                }

            }

            System.Console.WriteLine($"Prelievi added {add}, updated {upd}\n");

        }


        private static List<PrelievoLatteDto> PullRequest(DateTime lastDate)
        {

            string page = $"{baseUrl}/Api/PrelieviLatte/pull?timestamp={lastDate.ToString("yyyy-MM-dd")}";

            string result = RestRequestGet(page);

            return JsonConvert.DeserializeObject<List<PrelievoLatteDto>>(result);

        }

        protected static string RestRequestGet(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";

            WebResponse response = request.GetResponse();
            string text;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }

        protected static void RestRequestPost(string uri, string text)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(text);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

        }

        #endregion
    }
}

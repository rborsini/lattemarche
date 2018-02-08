using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core;
using LatteMarche.Service.DataOperation;
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

            TableSynchJob synchTable = new TableSynchJob(connectionString);

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
            Select select = new Select (connectionString);
            List<PrelievoLatteDto> prelieviPush = select.Select_Prelievi();
            return prelieviPush;
        }


        private static void InsertOrUpdate(List<PrelievoLatteDto> prelievi, string connectionString)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            int add = 0, upd = 0;

            foreach (PrelievoLatteDto prelievo in prelievi)
            {
                switch (prelievo.LastOperation)
                {
                    case OperationEnum.Added: InsertRecord(prelievo, connection); add++; break;
                    case OperationEnum.Updated: UpdateRecord(prelievo, connection); upd++; break;
                    default: break;
                }

            }

            System.Console.WriteLine($"Prelievi added {add}, updated {upd}\n");

        }

        /// <summary>
        /// Aggiorna il record in base all'ID del trasportatore, all'ID dell'allevamento e alla data del prelievo
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        private static void UpdateRecord(PrelievoLatteDto prelievo, SqlConnection connection)
        {
            string query = "UPDATE [dbo].[PRELIEVO_LATTE]                       " +
                           "SET                                                 " +
                                "DATA_CONSEGNA          =  @DataConsegna,       " +
                                "QUANTITA               =  @Quantita,           " +
                                "TEMPERATURA            =  @Temperatura,        " +
                                "DATA_ULTIMA_MUNGITURA  =  @DataUltimaMungitura," +
                                "SERIALE_LAB_ANALISI    =  @SerialeLab,        " +
                                "ID_DESTINATARIO        =  @IdDestinatario,     " +
                                "ID_ACQUIRENTE          =  @IdAcquirente,       " +
                                "ID_LABANALISI          =  @IdLabAnalisi,       " +
                                "NUMERO_MUNGITURE       =  @NumeroMungiture,    " +
                                "SCOMPARTO              =  @Scomparto,          " +
                                "LOTTO_CONSEGNA         =  @LottoConsegna       " +
                            "WHERE                                              " +
                                "ID_TRASPORTATORE       =  @IdTrasportatore     " + "AND " +
                                "ID_ALLEVAMENTO         =  @IdAllevamento       " + "AND " +
                                "DATA_PRELIEVO          =  @DataPrelievo        ";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(DateTimeSqlParameter("@DataPrelievo", prelievo.DataPrelievo));
            cmd.Parameters.Add(DateTimeSqlParameter("@DataConsegna", prelievo.DataConsegna));
            cmd.Parameters.Add(DecimalSqlParameter("@Quantita", prelievo.Quantita));
            cmd.Parameters.Add(DecimalSqlParameter("@Temperatura", prelievo.Temperatura));
            cmd.Parameters.Add(DateTimeSqlParameter("@DataUltimaMungitura", prelievo.DataUltimaMungitura));
            cmd.Parameters.Add(new SqlParameter("@SerialeLab", prelievo.SerialeLabAnalisi));
            cmd.Parameters.Add(IntSqlParameter("@IdAllevamento", prelievo.IdAllevamento));
            cmd.Parameters.Add(IntSqlParameter("@IdDestinatario", prelievo.IdDestinatario));
            cmd.Parameters.Add(IntSqlParameter("@IdAcquirente", prelievo.IdAcquirente));
            cmd.Parameters.Add(IntSqlParameter("@IdLabAnalisi", prelievo.IdLabAnalisi));
            cmd.Parameters.Add(IntSqlParameter("@IdTrasportatore", prelievo.IdTrasportatore));
            cmd.Parameters.Add(IntSqlParameter("@NumeroMungiture", prelievo.NumeroMungiture));
            cmd.Parameters.Add(new SqlParameter("@Scomparto", prelievo.Scomparto));
            cmd.Parameters.Add(new SqlParameter("@LottoConsegna", prelievo.LottoConsegna));
            cmd.ExecuteNonQuery();

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

        /// <summary>
        /// Inserisce un nuovo Record nella tabella prelievi
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        private static void InsertRecord(PrelievoLatteDto prelievo, SqlConnection connection)
        {

            string query = "INSERT INTO [dbo].[PRELIEVO_LATTE]" +
                               "([DATA_PRELIEVO], " +
                                "[DATA_CONSEGNA], " +
                                "[QUANTITA], " +
                                "[TEMPERATURA], " +
                                "[DATA_ULTIMA_MUNGITURA], " +
                                "[SERIALE_LAB_ANALISI], " +
                                "[ID_ALLEVAMENTO], " +
                                "[ID_DESTINATARIO], " +
                                "[ID_ACQUIRENTE], " +
                                "[ID_LABANALISI], " +
                                "[ID_TRASPORTATORE], " +
                                "[NUMERO_MUNGITURE], " +
                                "[SCOMPARTO], " +
                                "[LOTTO_CONSEGNA]) " +
                          "VALUES" +
                                "(@DataPrelievo, " +
                                " @DataConsegna, " +
                                " @Quantita, " +
                                " @Temperatura, " +
                                " @DataUltimaMungitura, " +
                                " @SerialeLab, " +
                                " @IdAllevamento, " +
                                " @IdDestinatario, " +
                                " @IdAcquirente, " +
                                " @IdLabAnalisi, " +
                                " @IdTrasportatore, " +
                                " @NumeroMungiture, " +
                                " @Scomparto, " +
                                " @LottoConsegna);";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.Add(DateTimeSqlParameter("@DataPrelievo", prelievo.DataPrelievo));
            cmd.Parameters.Add(DateTimeSqlParameter("@DataConsegna", prelievo.DataConsegna));
            cmd.Parameters.Add(DecimalSqlParameter("@Quantita", prelievo.Quantita));
            cmd.Parameters.Add(DecimalSqlParameter("@Temperatura", prelievo.Temperatura));
            cmd.Parameters.Add(DateTimeSqlParameter("@DataUltimaMungitura", prelievo.DataUltimaMungitura));
            cmd.Parameters.Add(new SqlParameter("@SerialeLab", prelievo.SerialeLabAnalisi));
            cmd.Parameters.Add(IntSqlParameter("@IdAllevamento", prelievo.IdAllevamento));
            cmd.Parameters.Add(IntSqlParameter("@IdDestinatario", prelievo.IdDestinatario));
            cmd.Parameters.Add(IntSqlParameter("@IdAcquirente", prelievo.IdAcquirente));
            cmd.Parameters.Add(IntSqlParameter("@IdLabAnalisi", prelievo.IdLabAnalisi));
            cmd.Parameters.Add(IntSqlParameter("@IdTrasportatore", prelievo.IdTrasportatore));
            cmd.Parameters.Add(IntSqlParameter("@NumeroMungiture", prelievo.NumeroMungiture));
            cmd.Parameters.Add(new SqlParameter("@Scomparto", prelievo.Scomparto));
            cmd.Parameters.Add(new SqlParameter("@LottoConsegna", prelievo.LottoConsegna));

            cmd.ExecuteNonQuery();
        }


        private static SqlParameter DecimalSqlParameter(string name, decimal? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.Decimal);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        private static SqlParameter DateTimeSqlParameter(string name, DateTime? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.DateTime);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        private static SqlParameter IntSqlParameter(string name, int? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.Int);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        #endregion
    }
}

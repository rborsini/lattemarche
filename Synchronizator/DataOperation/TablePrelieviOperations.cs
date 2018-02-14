using LatteMarche.Synch.DataType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Common.Logging;

namespace LatteMarche.Synch
{
    class TablePrelieviOperations
    {

        private string connectionString;
        private int DepthDays;

        private TextWriter log;

        public TablePrelieviOperations(string connectionString, int DepthDays, TextWriter log)
        {
            this.connectionString = connectionString;
            this.DepthDays = DepthDays;
            this.log = log;
        }

        public void InsertOrUpdate(List<Prelievo> prelievi, string connectionString)
        {
            int add = 0, upd = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            foreach (Prelievo prelievo in prelievi)
            {
                switch (prelievo.LastOperation)
                {
                    case OperationEnum.Added: InsertRecord(prelievo, connection); add++; break;
                    case OperationEnum.Updated: UpdateRecord(prelievo, connection); upd++; break;
                    default: break;
                }

            }

            this.log.WriteLine($"Prelievi added {add}, updated {upd}\n");

        }

        public List<Prelievo> SelectLastPrelievi(string connectionString)
        {
            List<Prelievo> prelieviPush = Select_Prelievi();
            return prelieviPush;
        }

        #region Pull

        /// <summary>
        /// Restituisce la lista di Prelievi letti dal Server
        /// </summary>
        /// <param name="lastDate"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public List<Prelievo> PullRequest(DateTime lastDate, string baseUrl)
        {
            string page = $"{baseUrl}/Api/PrelieviLatte/pull?timestamp={lastDate.ToString("yyyy-MM-dd")}";
            string result = RestRequestGet(page);
            
            List<Prelievo> prelievi = JsonConvert.DeserializeObject<List<Prelievo>>(result);

            this.log.WriteLine($"Prelievi Count {prelievi.Count()}\n");

            return prelievi;
        }

        /// <summary>
        /// Richiesta di GET per i prelievi tramite archiettura REST
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
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


        #endregion

        #region Push

        /// <summary>
        /// Invio Prelievi
        /// </summary>
        /// <param name="prelievi"></param>
        /// <param name="baseUrl"></param>
        public void PushRecords(List<Prelievo> prelievi, string baseUrl)
        {
            string page = baseUrl + "/Api/PrelieviLatte/push";
            var range = Convert.ToInt32(ConfigurationManager.AppSettings["range_synch"]);

            for (int i = 0; i <= prelievi.Count + range; i = i + range)
            {
                string prelievoJson = JsonConvert.SerializeObject(prelievi.Skip(i).Take(range));

                RestRequestPost(page, prelievoJson);

                System.Console.Write($"{1 + (i / range)}, ");

            }
            this.log.WriteLine ($"Prelievi sended n:{prelievi.Count}");

        }

        /// <summary>
        /// Richiesta di Post per i prelievi tramite architettura Rest 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="text"></param>
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

        #region SQL_PrelieviLatte_Operation
        /// <summary>
        /// Ricerca prelievi locali degli ultimi n giorni
        /// </summary>
        /// <param name="depthDays">Profondità in gg</param>
        /// <returns></returns>
        private List<Prelievo> Select_Prelievi()
        {
            List<Prelievo> righe = new List<Prelievo>();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "SELECT " +
                                "ID_PRELIEVO, " +
                                "DATA_PRELIEVO, " +
                                "DATA_CONSEGNA, " +
                                "QUANTITA, " +
                                "TEMPERATURA, " +
                                "DATA_ULTIMA_MUNGITURA, " +
                                "SERIALE_LAB_ANALISI, " +
                                "ID_ALLEVAMENTO, " +
                                "ID_DESTINATARIO, " +
                                "ID_ACQUIRENTE, " +
                                "ID_LABANALISI, " +
                                "ID_TRASPORTATORE, " +
                                "NUMERO_MUNGITURE, " +
                                "SCOMPARTO, " +
                                "LOTTO_CONSEGNA " +
                           "FROM [dbo].[PRELIEVO_LATTE] " +
                           "WHERE " +
                                "DATA_PRELIEVO > @Data";

            SqlCommand selectCommand = new SqlCommand(query, connection);

            selectCommand.Parameters.AddWithValue("@Data", DateTime.Today.AddDays(-this.DepthDays));

            SqlDataReader reader = selectCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var riga = new Prelievo();
                    riga.Id = reader.GetInt32(0);
                    riga.DataPrelievo       = reader.IsDBNull(1)  ? (DateTime?) null : reader.GetDateTime(1);
                    riga.DataConsegna       = reader.IsDBNull(2)  ? (DateTime?) null : reader.GetDateTime(2);
                    riga.Quantita           = reader.IsDBNull(3)  ? (decimal?)  null : reader.GetDecimal(3);
                    riga.Temperatura        = reader.IsDBNull(4)  ? (decimal?)  null : reader.GetDecimal(4);
                    riga.DataUltimaMungitura= reader.IsDBNull(5)  ? (DateTime?) null : reader.GetDateTime(5);
                    riga.SerialeLabAnalisi  = reader.IsDBNull(6)  ?             null : reader.GetString(6);
                    riga.IdAllevamento      = reader.IsDBNull(7)  ? (int?)      null : reader.GetInt32(7);
                    riga.IdDestinatario     = reader.IsDBNull(8)  ? (int?)      null : reader.GetInt32(8);
                    riga.IdAcquirente       = reader.IsDBNull(9)  ? (int?)      null : reader.GetInt32(9);
                    riga.IdLabAnalisi       = reader.IsDBNull(10) ? (int?)      null : reader.GetInt32(10);
                    riga.IdTrasportatore    = reader.IsDBNull(11) ? (int?)      null : reader.GetInt32(11);
                    riga.NumeroMungiture    = reader.IsDBNull(12) ? (int?)      null : reader.GetInt32(12);
                    riga.Scomparto          = reader.IsDBNull(13) ?             null : reader.GetString(13);
                    riga.LottoConsegna      = reader.IsDBNull(14) ?             null : reader.GetString(14);

                    righe.Add(riga);
                }
            }

            this.log.WriteLine($"Selected {righe.Count} prelievi");

            return righe;
        }

        /// <summary>
        /// Aggiorna il record in base all'ID del trasportatore, all'ID dell'allevamento e alla data del prelievo
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        public void UpdateRecord(Prelievo prelievo, SqlConnection connection)
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

        /// <summary>
        /// Inserisce un nuovo Record nella tabella prelievi
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        public void InsertRecord(Prelievo prelievo, SqlConnection connection)
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
        #endregion

        #region SqlDataController

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

        #endregion //TODO: Crea una classe e usa l'ereditarietà


    }
}

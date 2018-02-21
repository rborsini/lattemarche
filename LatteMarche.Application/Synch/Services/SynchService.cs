using LatteMarche.Application.PrelieviLatte.Interfaces;
using LatteMarche.Application.Synch.Interfaces;
using LatteMarche.Common;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Synch.Services
{
    public class SynchService : ISynchService
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["OldDbContext"].ConnectionString;
        private int DepthDays { get { return Convert.ToInt32(ConfigurationManager.AppSettings["days_depth"]); } }

        private IPrelieviLatteService prelieviLatteService;

        public SynchService(IPrelieviLatteService prelieviLatteService)
        {
            this.prelieviLatteService = prelieviLatteService;
        }

        /// <summary>
        /// Download dal cloud verso il server locale
        /// </summary>
        public void Pull()
        {
            // recupero ultima data sincronizzazione
            DateTime lastTimeStamp = GetDateLastSynch();

            // recupero record da database in cloud
            List<PrelievoLatte> prelievi = this.prelieviLatteService.Pull(lastTimeStamp);

            // salvataggio database server locale
            InsertOrUpdate(prelievi, connectionString);
        }

        /// <summary>
        /// Upload dal server locale verso il cloud
        /// </summary>
        public List<PrelievoLatte> Push()
        {
            // recupero record da server locale
            List<PrelievoLatte> prelievi = Select_Prelievi();

            // salvataggio su database cloud
            List<PrelievoLatte> nuoviPrelievi = this.prelieviLatteService.Push(prelievi);

            // aggiornamento tabella synch server locale
            UpdateSynchTable(SynchTypeEnum.Push);

            return nuoviPrelievi;
        }

        /// <summary>
        /// Restituisce la data dell'ultima sincronizzazione
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateLastSynch()
        {
            DateTime result = DateTime.Today.AddDays(-30);

            SqlConnection connection = new SqlConnection(this.connectionString);
            connection.Open();

            string query = "SELECT " +
                                "ID, " +
                                "TIMESTAMP, " +
                                "NOTE, " +
                                "TIPO_OPERAZIONE " +
                            "FROM [dbo].[SYNCH] " +
                            "WHERE ID = (SELECT MAX(ID) FROM [dbo].[SYNCH])";

            SqlCommand selectCommand = new SqlCommand(query, connection);
            SqlDataReader reader = selectCommand.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read()) result = reader.GetDateTime(1);

            //this.log.WriteLine($"Last Timestamp {result.ToString()}\n");

            return result;
        }


        public void InsertOrUpdate(List<PrelievoLatte> prelievi, string connectionString)
        {
            int add = 0, upd = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            foreach (PrelievoLatte prelievo in prelievi)
            {
                switch (prelievo.LastOperation)
                {
                    case OperationEnum.Added: InsertRecord(prelievo, connection); add++; break;
                    case OperationEnum.Updated: UpdateRecord(prelievo, connection); upd++; break;
                    default: break;
                }

            }

            //this.log.WriteLine($"Prelievi added {add}, updated {upd}\n");

        }

        /// <summary>
        /// Ricerca prelievi locali degli ultimi n giorni
        /// </summary>
        /// <param name="depthDays">Profondità in gg</param>
        /// <returns></returns>
        private List<PrelievoLatte> Select_Prelievi()
        {
            List<PrelievoLatte> righe = new List<PrelievoLatte>();

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
                                "DATA_PRELIEVO >= @Data";

            SqlCommand selectCommand = new SqlCommand(query, connection);

            selectCommand.Parameters.AddWithValue("@Data", DateTime.Today.AddDays(-this.DepthDays));

            //DateTime inizio = new DateTime(2000, 1, 1);
            //DateTime fine = inizio.AddYears(10);

            //selectCommand.Parameters.AddWithValue("@DataInizio", inizio);
            //selectCommand.Parameters.AddWithValue("@DataFine", fine);

            SqlDataReader reader = selectCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var riga = new PrelievoLatte();
                    riga.Id = reader.GetInt32(0);
                    riga.DataPrelievo = reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1);
                    riga.DataConsegna = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2);
                    riga.Quantita = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3);
                    riga.Temperatura = reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4);
                    riga.DataUltimaMungitura = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                    riga.SerialeLabAnalisi = reader.IsDBNull(6) ? null : reader.GetString(6);
                    riga.IdAllevamento = reader.IsDBNull(7) ? (int?)null : reader.GetInt32(7);
                    riga.IdDestinatario = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                    riga.IdAcquirente = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9);
                    riga.IdLabAnalisi = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10);
                    riga.IdTrasportatore = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11);
                    riga.NumeroMungiture = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12);
                    riga.Scomparto = reader.IsDBNull(13) ? null : reader.GetString(13);
                    riga.LottoConsegna = reader.IsDBNull(14) ? null : reader.GetString(14);

                    righe.Add(riga);
                }
            }

            //this.log.WriteLine($"Selected {righe.Count} prelievi");

            return righe;
        }

        /// <summary>
        /// Aggiorna il record in base all'ID del trasportatore, all'ID dell'allevamento e alla data del prelievo
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        public void UpdateRecord(PrelievoLatte prelievo, SqlConnection connection)
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
        public void InsertRecord(PrelievoLatte prelievo, SqlConnection connection)
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


        /// <summary>
        /// Inserisce nella tabella Synch la data dell'ultima sincronizzazione
        /// </summary>
        public void UpdateSynchTable(SynchTypeEnum OperationType)
        {

            SqlConnection connection = new SqlConnection(this.connectionString);
            connection.Open();

            string query = "INSERT INTO [dbo].[SYNCH]" +
                               "([TIMESTAMP], " +
                                "[NOTE], " +
                                "[TIPO_OPERAZIONE]) " +
                           "VALUES " +
                                "(@DataOdierna, " +
                                "@Note, " +
                                "@TipoOperazione)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.Add(DateTimeSqlParameter("@DataOdierna", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@Note", "nothing"));
            cmd.Parameters.Add(new SqlParameter("@TipoOperazione", OperationType.ToString()));

            //this.log.WriteLine($"Synch Table updated with {OperationType.ToString()} operation\n");

            cmd.ExecuteNonQuery();
        }

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

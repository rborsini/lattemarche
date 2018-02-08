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
using System.Data.SqlClient;
using System.Configuration;

namespace LatteMarche.Service.Jobs
{
    class TablePrelieviOperations
    {
        private int DepthDays { get { return Convert.ToInt32(ConfigurationManager.AppSettings["days_depth"]); } }

        private string connectionString;


        public TablePrelieviOperations(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Ricerca prelievi locali degli ultimi n giorni
        /// </summary>
        /// <param name="depthDays">Profondità in gg</param>
        /// <returns></returns>
        public List<PrelievoLatteDto> Select_Prelievi()
        {
            List<PrelievoLatteDto> righe = new List<PrelievoLatteDto>();

            SqlConnection connection = new SqlConnection(this.connectionString);
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
                    var riga = new PrelievoLatteDto();
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

            System.Console.WriteLine($"Selected {righe.Count} prelievi");

            return righe;
        }

        /// <summary>
        /// Aggiorna il record in base all'ID del trasportatore, all'ID dell'allevamento e alla data del prelievo
        /// </summary>
        /// <param name="prelievo"></param>
        /// <param name="connection"></param>
        public void UpdateRecord(PrelievoLatteDto prelievo, SqlConnection connection)
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
        public void InsertRecord(PrelievoLatteDto prelievo, SqlConnection connection)
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


    }
}

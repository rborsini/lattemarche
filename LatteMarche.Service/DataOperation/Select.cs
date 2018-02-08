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

namespace LatteMarche.Service.DataOperation.Jobs
{
    class Select
    {
        private int DepthDays { get { return Convert.ToInt32(ConfigurationManager.AppSettings["days_depth"]); } }

        private string connectionString;


        public Select(string connectionString)
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
    }
}

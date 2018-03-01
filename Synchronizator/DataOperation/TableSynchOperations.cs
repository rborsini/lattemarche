using LatteMarche.Synch.DataType;
using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Common.Logging;
using System.IO;

namespace LatteMarche.Synch
{
    class TableSynchOperations
    {
        private string connectionString;
        private TextWriter log;

        public TableSynchOperations(string connectionString, TextWriter log)
        {
            this.connectionString = connectionString;
            this.log = log;

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

            this.log.WriteLine($"Last Timestamp {result.ToString()}\n");

            return result;
        }


        /// <summary>
        /// Inserisce nella tabella Synch la data dell'ultima sincronizzazione
        /// </summary>
        public void UpdateSynchTable(OperationTypeEnum OperationType)
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

            this.log.WriteLine($"Synch Table updated with {OperationType.ToString()} operation\n");

            cmd.ExecuteNonQuery();
        }

        #region SqlDataController

        private static SqlParameter DateTimeSqlParameter(string name, DateTime? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.DateTime);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        #endregion
    }
}

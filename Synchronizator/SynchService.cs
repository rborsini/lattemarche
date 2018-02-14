using LatteMarche.Synch.DataType;
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
using Common.Logging;

namespace LatteMarche.Synch
{
    public class SynchService
    {

        private string connectionString;
        private int daysDepth;
        private string baseUrl;
        private TextWriter log;

        public SynchService(string connectionString, int daysDepth, string baseUrl, TextWriter log)
        {
            this.connectionString = connectionString;
            this.daysDepth = daysDepth;
            this.baseUrl = baseUrl;
            this.log = log;
        }

        /// <summary>
        /// Effettua il Pull dei prelievi dal server Azure al Dabase Locale in base all'ultima data di sincronizzazione. Li inserisce nel database locale ed aggiorna la tabella Synch.
        /// </summary>
        public void Pull()
        {
            TableSynchOperations synchTable = new TableSynchOperations(connectionString, log);
            TablePrelieviOperations operation = new TablePrelieviOperations(connectionString, daysDepth, log);

            DateTime lastTimeStamp = synchTable.GetDateLastSynch();
            List<Prelievo> prelievi = operation.PullRequest(lastTimeStamp, connectionString);
            operation.InsertOrUpdate(prelievi, connectionString);

            synchTable.UpdateSynchTable(OperationTypeEnum.Pull);
        }

        public void Push()
        {
            TablePrelieviOperations operation = new TablePrelieviOperations(connectionString, daysDepth, log);
            TableSynchOperations synchTable = new TableSynchOperations(connectionString, log);

            List<Prelievo> prelievi = operation.SelectLastPrelievi(connectionString);
            operation.PushRecords(prelievi, baseUrl);

            synchTable.UpdateSynchTable(OperationTypeEnum.Push);
        }
    }
}

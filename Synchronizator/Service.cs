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

namespace LatteMarche.Synch
{
    public class Service
    {

        private string connectionString;
        private int daysDepth;
        private string baseUrl;


        public Service(string connectionString, int daysDepth, string baseUrl)
        {
            this.connectionString = connectionString;
            this.daysDepth = daysDepth;
            this.baseUrl = baseUrl;
        }

        public void Pull()
        {
            TableSynchOperations synchTable = new TableSynchOperations(connectionString);
            TablePrelieviOperations operation = new TablePrelieviOperations(connectionString, daysDepth);

            DateTime lastTimeStamp = synchTable.GetDateLastSynch();
            System.Console.WriteLine($"Last Timestamp {lastTimeStamp.ToString()}\n");     

            List<Prelievo> prelievi = operation.PullRequest(lastTimeStamp, connectionString);

            System.Console.WriteLine($"Prelievi Count {prelievi.Count()}\n");

            operation.InsertOrUpdate(prelievi, connectionString);

            synchTable.UpdateSyncTable(OperationTypeEnum.Pull);
        }

        public void Push()
        {
            TablePrelieviOperations operation = new TablePrelieviOperations(connectionString, daysDepth);
            TableSynchOperations synchTable = new TableSynchOperations(connectionString);

            List<Prelievo> prelieviPush = operation.SelectLastPrelievi(connectionString);

            operation.PushRecords(prelieviPush, baseUrl);

            synchTable.UpdateSyncTable(OperationTypeEnum.Push);
        }
    }
}

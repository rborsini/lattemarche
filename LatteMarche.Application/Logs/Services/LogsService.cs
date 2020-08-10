using DelegateDecompiler;
using LatteMarche.Application.Logs.Dtos;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace LatteMarche.Application.Logs.Services
{
    public class LogsService : EntityService<LogRecord, long, LogRecordDto>, ILogsService
    {

        private string connectionString => ConfigurationManager.ConnectionStrings["LatteMarcheDbContext"].ConnectionString;

        public LogsService(IUnitOfWork uow)
            : base(uow)
        { }

        public void Delete(DateTime from)
        {
            var sql = "DELETE FROM Logs WHERE Date < @Date";

            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Date", from);

                cmd.ExecuteNonQuery();
            }

        }

        protected override LogRecord UpdateProperties(LogRecord viewEntity, LogRecord dbEntity)
        {
            throw new NotImplementedException();
        }
    }
}

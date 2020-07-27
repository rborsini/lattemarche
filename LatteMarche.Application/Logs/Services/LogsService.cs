using LatteMarche.Application.Logs.Dtos;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
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

        public LogsService(IUnitOfWork uow)
            : base(uow)
        { }

        public void Delete(DateTime from)
        {
            var connectionString = ((WeCode.Data.UnitOfWork)uow).Context.Database.Connection.ConnectionString;
            var sql = "DELETE FROM Logs WHERE Date < @Date";

            using (var connection = new SqlConnection(connectionString))
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

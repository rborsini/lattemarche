using LatteMarche.Application.Logs.Dtos;
using LatteMarche.Application.Logs.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Logs.Services
{
    public class LogsService : EntityService<LogRecord, long, LogRecordDto>, ILogsService
    {

        public LogsService(IUnitOfWork uow)
            : base(uow)
        { }

        protected override LogRecord UpdateProperties(LogRecord viewEntity, LogRecord dbEntity)
        {
            throw new NotImplementedException();
        }
    }
}

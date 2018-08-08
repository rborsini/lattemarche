using LatteMarche.Application.Logs.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Logs.Interfaces
{
    public interface ILogsService : IEntityService<LogRecord, long, LogRecordDto>
    {
    }
}

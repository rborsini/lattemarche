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
using System.Configuration;
using System.Data.SqlClient;

namespace LatteMarche.Service.Jobs
{
        public SqlParameter DecimalSqlParameter(string name, decimal? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.Decimal);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        public SqlParameter DateTimeSqlParameter(string name, DateTime? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.DateTime);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }

        public SqlParameter IntSqlParameter(string name, int? value)
        {
            SqlParameter parameter = new SqlParameter(name, System.Data.SqlDbType.Int);
            parameter.Value = (object)value ?? DBNull.Value;

            return parameter;
        }
}

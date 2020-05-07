using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Logs.Dtos
{
    public class LogRecordDto 
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Source { get; set; }
        public string HostName { get; set; }
        public string Identity { get; set; }
        public string Request { get; set; }
        public string Arguments { get; set; }
        public decimal Duration { get; set; }
    }



}

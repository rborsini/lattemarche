using AutoMapper;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Destinatari.Dtos
{
    public class DestinatarioDto 
    {
        public int Id { get; set; }

        public string RagioneSociale { get; set; }

        public string P_IVA { get; set; }

        public string Indirizzo { get; set; }
        public string SiglaProvincia { get; set; }
        public int? IdComune { get; set; }

        public string Stabilimento { get; set; }

        public int? IdSitraStabilimentoCaseificio { get; set; }

    }


}

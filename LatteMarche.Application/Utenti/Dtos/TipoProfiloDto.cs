using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Dtos
{

    public enum ProfiloEnum
    {
        Admin = 1,
        Redatore =  2,
        Allevatore = 3,
        Laboratorio = 4,
        Trasportatore = 5,
        Destinatario = 6,
        Acquirente = 7,
        Cessionario =  8
    }

    public class TipoProfiloDto 
    {
        public int Id { get; set; }
        public string Descrizione { get; set; }
        
    }


}

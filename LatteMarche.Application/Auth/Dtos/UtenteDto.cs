using System;
using AutoMapper;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Auth.Dtos
{
    public class UtenteDto 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        public string PivaCF { get; set; }
        public string Indirizzo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Abilitato { get; set; }
        public bool Visibile { get; set; }
        public string RagioneSociale { get; set; }
        public string CodiceAllevatore { get; set; }
        public int QuantitaLatte { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }
        public string Sesso { get; set; }       
        public string NumeroComunicazione { get; set; }
        public string Note { get; set; }

        public string SiglaProvincia { get; set; }
        public int IdComune { get; set; }
        public int IdProfilo { get; set; }
        public int IdTipoLatte { get; set; }
    }

}

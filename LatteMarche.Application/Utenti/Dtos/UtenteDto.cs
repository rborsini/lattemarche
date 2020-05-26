using System;
using System.Collections.Generic;
using AutoMapper;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Autocisterne.Dtos;
using LatteMarche.Core.Models;

namespace LatteMarche.Application.Utenti.Dtos
{
    public class UtenteDto 
    {
        public int Id { get; set; }

        public bool Abilitato { get; set; }
        public bool Visibile { get; set; }

        public string RagioneSociale { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        public string NomeCompleto => $"{this.Cognome} {this.Nome}";

        public int IdProfilo { get; set; }

        public string Profilo
        {
            get
            {
                switch (this.IdProfilo)
                {
                    case 1:
                        return "Admin";
                    case 2:
                        return "Redatore";
                    case 3:
                        return "Allevatore";
                    case 4:
                        return "Laboratorio";
                    case 5:
                        return "Trasportatore";
                    case 6:
                        return "Destinatario";
                    case 7:
                        return "Acquirente";
                    case 8:
                        return "Cessionario";
                    default:
                        return "";
                }
            }
        }

        public string Sesso { get; set; }

        public string PivaCF { get; set; }
        public string Indirizzo { get; set; }
        public string SiglaProvincia { get; set; }
        public int? IdComune { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }


        public int? IdTipoLatte { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string Note { get; set; }


        public int? IdAcquirente { get; set; }
        public int? IdDestinatario { get; set; }
        public int? IdCessionario { get; set; }
        public int? IdAziendaTrasporti { get; set; }

        public List<AllevamentoDto> Allevamenti { get; set; }

        public List<AutocisternaDto> Autocisterne { get; set; }

        public UtenteDto()
        {
            this.Allevamenti = new List<AllevamentoDto>();
            this.Autocisterne = new List<AutocisternaDto>();
        }

    }

    public class UtentiSearchDto
    {
        public int? IdProfilo { get; set; }
    }

}

using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Sitra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Sitra.Services
{
    public class SitraService : ISitraService
    {
        public List<LottoDto> InvioLotti(List<LottoDto> lotti)
        {
            throw new NotImplementedException();
            // Conversione dal modello LatteMarche a modello Sitra

            // Recupero token

            // Invio lotti

            // Aggiornamento campi lotti (Es. Inviato, Errore, Messaggio, Timestamp, CodiceSitra)

        }
    }
}

using LatteMarche.Application.Mobile.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Mobile.Interfaces
{
    public interface IMobileService
    {
        /// <summary>
        /// Registrazione device
        /// </summary>
        /// <param name="dispostivo"></param>
        /// <returns></returns>
        DispositivoDto Register(DispositivoDto dispostivo);

        /// <summary>
        /// Scaricamento dati di anagrafica e lookup
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        DownloadDto Download(string imei);

        /// <summary>
        /// Caricamento dati prelievi latte
        /// </summary>
        /// <param name="dow"></param>
        void Upload(UploadDto dow);
    }
}

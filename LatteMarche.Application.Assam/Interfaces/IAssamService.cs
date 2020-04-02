using LatteMarche.Application.Assam.Models;
using LatteMarche.Application.Assam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Assam.Interfaces
{
    public interface IAssamService
    {

        /// <summary>
        /// Verifica presenza nuovi messaggi nella casella di posta
        /// </summary>
        /// <param name="mailOptions">Configurazione casella posta</param>
        /// <param name="mailFilters">Filtri ricerca mail</param>
        /// <param name="ftpOptions">Configurazione cartella ftp backup</param>
        /// <returns></returns>
        List<Report> CheckMailBox(MailOptions mailOptions, MailFilters mailFilters, FtpOptions ftpOptions = null);


    }
}

using AutoMapper;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.Assam.Interfaces;
using LatteMarche.Application.Assam.Models;
using LatteMarche.Application.Assam.Services;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.AnalisiLatte.Services
{
    public class AnalisiService : EntityService<Analisi, string, AnalisiDto>, IAnalisiService
    {
        #region Constants

        private string hostName => ConfigurationManager.AppSettings["Assam.Mail.Hostname"];
        private int port => Convert.ToInt32(ConfigurationManager.AppSettings["Assam.Mail.Port"]);
        private string username => ConfigurationManager.AppSettings["Assam.Mail.Username"];
        private string password => ConfigurationManager.AppSettings["Assam.Mail.Password"];
        private string from => ConfigurationManager.AppSettings["Assam.Mail.From"];
        private int depth => Convert.ToInt32(ConfigurationManager.AppSettings["Assam.Mail.Depth"]);

        private string ftpUrl => ConfigurationManager.AppSettings["Assam.Ftp.Url"];
        private string ftpUsername => ConfigurationManager.AppSettings["Assam.Ftp.Username"];
        private string ftpPassword => ConfigurationManager.AppSettings["Assam.Ftp.Password"];

        #endregion

        #region Fields

        private IAssamService assamService;
        private IAllevamentiService allevamentiService;

        #endregion

        #region Constructor

        public AnalisiService(IUnitOfWork uow, IAssamService assamService, IAllevamentiService allevamentiService)
            : base(uow)
        {
            this.assamService = assamService;
            this.allevamentiService = allevamentiService;
        }

        #endregion

        #region Methods

        public List<AnalisiDto> Search(AnalisiSearchDto searchDto)
        {
            var query = this.repository.GetAll();

            // IdProduttore
            if (searchDto.IdProduttore.HasValue)
                query = query.Where(a => a.IdProduttore == searchDto.IdProduttore);

            // Codice Produttore
            if (!String.IsNullOrEmpty(searchDto.CodiceProduttore))
                query = query.Where(a => a.CodiceProduttore == searchDto.CodiceProduttore);

            // IdAllevamento
            if (searchDto.IdAllevamento.HasValue)
                query = query.Where(a => a.IdAllevamento == searchDto.IdAllevamento);

            // Codice ASL
            if (!String.IsNullOrEmpty(searchDto.CodiceAsl))
                query = query.Where(a => a.CodiceASL == searchDto.CodiceAsl);

            return ConvertToDtoList(query.ToList());
        }

        public void Synch()
        {
            var mailOptions = new MailOptions() { HostName = this.hostName, Port = this.port, Username = this.username, Password = this.password };
            var mailFilters = new MailFilters() { From = this.from, Since = DateTime.Today.AddDays(-this.depth), Before = DateTime.Now };
            var ftpOptions = String.IsNullOrEmpty(ftpUrl) ? null : new FtpOptions() { Url = this.ftpUrl, Username = this.ftpUsername, Password = this.ftpPassword };

            // download reports 
            var reports = this.assamService.CheckMailBox(mailOptions, mailFilters, ftpOptions);

            // salvataggio report
            foreach(var report in reports)
            {
                try
                {
                    Save(report);
                }
                catch(Exception exc)
                {
                    Console.WriteLine("Save error", exc);
                }
            }

        }

        private void Save(Report report)
        {
            
            var analisiList = Mapper.Map<List<AnalisiDto>>(report.Analisi);

            foreach (var analisi in analisiList)
            {
                // mapping dei campi provenienti dalla testata del report (automapper lì non ci arriva)
                analisi.CodiceProduttore = report.Produttore_Codice;
                analisi.NomeProduttore = report.Produttore_Nome;
                analisi.TipoLatte_Descr = report.TipoLatte;

                // aggancio con produttore e allevamento
                var codiceAllevatore = report.Produttore_Codice.StartsWith("A") ? report.Produttore_Codice.Substring(1) : report.Produttore_Codice; // nel file excel il codice allevatore è preceduto a 'A'
                analisi.IdProduttore = GetIdProduttore(codiceAllevatore, analisi.CodiceASL);
                analisi.IdAllevamento = GetIdAllevamento(analisi.CodiceASL);

                if(analisi.IdProduttore.HasValue || analisi.IdAllevamento.HasValue)
                {
                    // verifica presenza analisi nel database
                    var existingAnalisi = this.repository.GetById(analisi.Id);
                    if (existingAnalisi == null)
                        this.Create(analisi);
                    else
                        this.Update(analisi);
                }

            }


        }


        private int? GetIdProduttore(string codiceProduttore, string codiceAsl)
        {
            if (String.IsNullOrEmpty(codiceProduttore) && String.IsNullOrEmpty(codiceAsl))
                return (int?)null;

            // ricerca per doppio campo CodiceAllevatore e CodiceASL
            var produttori = this.allevamentiService.Search(new AllevamentiSearchDto() { CodiceAllevatore = codiceProduttore, CodiceAsl = codiceAsl });
            if (produttori.Count == 1)
                return produttori[0].IdUtente;

            // ricerca per codice produttore
            produttori = this.allevamentiService.Search(new AllevamentiSearchDto() { CodiceAllevatore = codiceProduttore });
            if (produttori.Count == 1)
                return produttori[0].IdUtente;

            // ricerca per codice asl
            produttori = this.allevamentiService.Search(new AllevamentiSearchDto() { CodiceAsl = codiceAsl });
            if (produttori.Count == 1)
                return produttori[0].IdUtente;

            return (int?)null;
        }

        private int? GetIdAllevamento(string codiceASL)
        {
            if (String.IsNullOrEmpty(codiceASL))
                return (int?)null;

            var allevamenti = this.allevamentiService.Search(new AllevamentiSearchDto() { CodiceAsl = codiceASL });
            if (allevamenti.Count == 1)
                return allevamenti[0].Id;

            return (int?)null;
        }

        protected override Analisi UpdateProperties(Analisi viewEntity, Analisi dbEntity)
        {
            dbEntity.CodiceASL = viewEntity.CodiceASL;

            dbEntity.CodiceProduttore = viewEntity.CodiceProduttore;
            dbEntity.IdAllevamento = viewEntity.IdAllevamento;
            dbEntity.IdProduttore = viewEntity.IdProduttore;
            dbEntity.NomeProduttore = viewEntity.NomeProduttore;
            dbEntity.TipoLatte = viewEntity.TipoLatte;
            dbEntity.TipoLatte_Descr = viewEntity.TipoLatte_Descr;

            dbEntity.DataAccettazione = viewEntity.DataAccettazione;
            dbEntity.DataPrelievo = viewEntity.DataPrelievo;
            dbEntity.DataRapportoDiProva = viewEntity.DataRapportoDiProva;

            return dbEntity;
        }





        #endregion

    }
}

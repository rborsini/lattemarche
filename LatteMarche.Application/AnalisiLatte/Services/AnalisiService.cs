using AutoMapper;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.AnalisiLatte.Dtos;
using LatteMarche.Application.AnalisiLatte.Interfaces;
using LatteMarche.Application.Assam.Interfaces;
using LatteMarche.Application.Assam.Models;
using LatteMarche.Application.Assam.Services;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

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

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<ValoreAnalisi, long> valoriRepository;

        private IAssamService assamService;
        
        #endregion

        #region Constructor

        public AnalisiService(IUnitOfWork uow, IAssamService assamService)
            : base(uow)
        {
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.valoriRepository = this.uow.Get<ValoreAnalisi, long>();

            this.assamService = assamService;            
        }

        #endregion

        #region Methods

        public List<AnalisiDto> Search(AnalisiSearchDto searchDto)
        {
            var query = this.repository.Query;

            // Campione
            if (!String.IsNullOrEmpty(searchDto.Campione))
                query = query.Where(a => a.Id == searchDto.Campione);

            // IdProduttore
            if (searchDto.IdProduttore.HasValue)
                query = query.Where(a => a.IdProduttore == searchDto.IdProduttore);

            // Codice Produttore
            if (!String.IsNullOrEmpty(searchDto.CodiceProduttore))
                query = query.Where(a => a.CodiceProduttore == searchDto.CodiceProduttore);

            // IdAllevamento
            if (searchDto.IdAllevamento.HasValue && searchDto.IdAllevamento.Value != 0)
                query = query.Where(a => a.IdAllevamento == searchDto.IdAllevamento);

            // Codice ASL
            if (!String.IsNullOrEmpty(searchDto.CodiceAsl))
                query = query.Where(a => a.CodiceASL == searchDto.CodiceAsl);

            // Periodo Prelievo
            if (searchDto.DataPeriodoInizio.HasValue || searchDto.DataPeriodoFine.HasValue)
            {
                DateTime from = searchDto.DataPeriodoInizio.HasValue ? searchDto.DataPeriodoInizio.Value : DateTime.MinValue;
                DateTime to = searchDto.DataPeriodoFine.HasValue ? searchDto.DataPeriodoFine.Value.AddDays(1) : DateTime.MaxValue;

                query = query.Where(p => from <= p.DataPrelievo && p.DataPrelievo < to);
            }

            return ConvertToDtoList(query.ToList());
        }

        public override AnalisiDto Update(AnalisiDto model)
        {
            var analisi = this.repository.GetById(model.Id);
            var viewValori = Mapper.Map<List<ValoreAnalisi>>(model.Valori);
            var dbValori = analisi != null ? analisi.Valori : new List<ValoreAnalisi>();
            UpdateValori(viewValori, dbValori);

            return base.Update(model);
        }

        private void UpdateValori(List<ValoreAnalisi> viewValori, List<ValoreAnalisi> dbValori)
        {
            foreach(var viewItem in viewValori)
            {
                var existingItem = dbValori.FirstOrDefault(v => v.Nome == viewItem.Nome);
                if(existingItem != null)
                {
                    existingItem.Valore = viewItem.Valore;
                    existingItem.FuoriSoglia = viewItem.FuoriSoglia;

                    this.valoriRepository.Update(existingItem);
                }
            }

            this.uow.SaveChanges();
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
            
            var analisiList = Mapper.Map<List<AnalisiDto>>(report.Analisi.Where(a => !String.IsNullOrEmpty(a.Campione)));

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
            var produttori = this.allevamentiRepository.DbSet.Where(a => a.Utente.CodiceAllevatore == codiceProduttore && a.CodiceAsl == codiceAsl);
            if (produttori.Count() == 1)
                return produttori.First().IdUtente;

            // ricerca per codice produttore
            produttori = this.allevamentiRepository.DbSet.Where(a => a.Utente.CodiceAllevatore == codiceProduttore);
            if (produttori.Count() == 1)
                return produttori.First().IdUtente;

            // ricerca per codice asl
            produttori = this.allevamentiRepository.DbSet.Where(a => a.CodiceAsl == codiceAsl);
            if (produttori.Count() == 1)
                return produttori.First().IdUtente;

            return (int?)null;
        }

        private int? GetIdAllevamento(string codiceASL)
        {
            if (String.IsNullOrEmpty(codiceASL))
                return (int?)null;

            var allevamenti = this.allevamentiRepository.DbSet.Where(a => a.CodiceAsl == codiceASL);
            if (allevamenti.Count() == 1)
                return allevamenti.First().IdUtente;

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

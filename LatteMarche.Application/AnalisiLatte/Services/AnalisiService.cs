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
using log4net;
using LatteMarche.Application.Common.Dtos;

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
        
        private string ftpUrl => ConfigurationManager.AppSettings["Assam.Ftp.Url"];
        private string ftpUsername => ConfigurationManager.AppSettings["Assam.Ftp.Username"];
        private string ftpPassword => ConfigurationManager.AppSettings["Assam.Ftp.Password"];

        #endregion

        #region Fields

        private static ILog log = LogManager.GetLogger(typeof(AnalisiService));

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<ValoreAnalisi, long> valoriRepository;

        private IAssamService assamService;
        private IUtentiService utentiService;
        
        #endregion

        #region Constructor

        public AnalisiService(IUnitOfWork uow, IMapper mapper, IAssamService assamService, IUtentiService utentiService)
            : base(uow, mapper)
        {
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.valoriRepository = this.uow.Get<ValoreAnalisi, long>();

            this.assamService = assamService;
            this.utentiService = utentiService;
        }

        #endregion

        #region Methods

        public DropDownDto DropDown()
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.repository.Query
                .Select(a => a.Categoria)
                .Distinct()
                .Select(c => new DropDownItem()
                {
                    Value = c,
                    Text = c
                })
                .OrderBy(i => i.Text)
                .ToList();

            return dropdown;
        }

        public List<AnalisiDto> Search(AnalisiSearchDto searchDto, int idUtente)
        {
            IQueryable<Analisi> query = AnalisiAutorizzate(idUtente);

            query = query.Where(a => a.IdAllevamento.HasValue || a.IdProduttore.HasValue);

            // Categoria
            if(!String.IsNullOrEmpty(searchDto.Categoria))
                query = query.Where(a => a.Categoria == searchDto.Categoria);

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

        private IQueryable<Analisi> AnalisiAutorizzate(int idUtente)
        {
            var query = this.repository.DbSet;
            var utente = this.utentiService.Details(idUtente);

            switch (utente.IdProfilo)
            {
                case 1:     // Admin
                    return query;

                case 7:     // Acquirente

                    var allevamentiIds = this.uow.Get<PrelievoLatte, int>().DbSet
                        .Where(p => p.IdAcquirente == utente.IdAcquirente)
                        .Select(p => p.IdAllevamento.Value)
                        .Distinct()
                        .ToList();

                    return query.Where(a => allevamentiIds.Contains(a.IdAllevamento.Value));

                case 3:     // allevamenti associati all'utente

                    allevamentiIds = this.allevamentiRepository.DbSet
                        .Where(a => a.IdUtente == idUtente)
                        .Select(a => a.Id).ToList();

                    return query.Where(a => allevamentiIds.Contains(a.IdAllevamento.Value));

                case 4:     // Laboratorio

                    return query;

                case 5:     // Trasportatore
                    
                    return query.Where(a => a.IdAllevamento == -1);

                case 6:     // Destinatario

                    allevamentiIds = this.uow.Get<PrelievoLatte, int>().DbSet
                        .Where(p => p.IdDestinatario == utente.IdDestinatario)
                        .Select(p => p.IdAllevamento.Value)
                        .Distinct()
                        .ToList();

                    return query.Where(a => allevamentiIds.Contains(a.IdAllevamento.Value));


                case 8:     // Cessionario

                    allevamentiIds = this.uow.Get<PrelievoLatte, int>().DbSet
                        .Where(p => p.IdCessionario == utente.IdCessionario)
                        .Select(p => p.IdAllevamento.Value)
                        .Distinct()
                        .ToList();

                    return query.Where(a => allevamentiIds.Contains(a.IdAllevamento.Value));

                default:
                    return query;

            }


        }

        public override AnalisiDto Update(AnalisiDto model)
        {
            var analisi = this.repository.GetById(model.Id);
            var viewValori = this.mapper.Map<List<ValoreAnalisi>>(model.Valori);
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

        public List<Report> Synch()
        {

            var mailOptions = new MailOptions() { HostName = this.hostName, Port = this.port, Username = this.username, Password = this.password };
            var mailFilters = new MailFilters() { From = this.from };

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
                    log.Error(exc);
                }
            }

            return reports;
        }

        private void Save(Report report)
        {
            
            var analisiList = this.mapper.Map<List<AnalisiDto>>(report.Analisi.Where(a => !String.IsNullOrEmpty(a.Campione)));

            foreach (var analisi in analisiList)
            {
                // mapping dei campi provenienti dalla testata del report (automapper lì non ci arriva)
                analisi.CodiceProduttore = report.Produttore_Codice;
                analisi.NomeProduttore = report.Produttore_Nome;
                analisi.TipoLatte_Descr = report.TipoLatte;
                analisi.Categoria = report.Categoria;

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
                return allevamenti.First().Id;

            return (int?)null;
        }

        protected override Analisi UpdateProperties(Analisi viewEntity, Analisi dbEntity)
        {
            dbEntity.CodiceASL = viewEntity.CodiceASL;

            dbEntity.Categoria = viewEntity.Categoria;
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

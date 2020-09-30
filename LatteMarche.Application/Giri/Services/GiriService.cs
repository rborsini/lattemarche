using System;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using WeCode.Data.Interfaces;
using WeCode.Application;
using LatteMarche.EntityFramework;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Application.Trasportatori.Interfaces;

namespace LatteMarche.Application.Giri.Services
{

    public class GiriService: EntityService<Giro, int, GiroDto>, IGiriService
    {

        #region Fields

        private IRepository<Giro, int> giriRepository;
        private IRepository<Allevamento, int> allevamentiRepository;
        private ITrasportatoriService trasportatoriService;

        #endregion

        #region Constructor

        public GiriService(IUnitOfWork uow, ITrasportatoriService trasportatoriService)
            : base(uow)
        {
            this.giriRepository = this.uow.Get<Giro, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();

            this.trasportatoriService = trasportatoriService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento dettaglio Giro (items compresi)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override GiroDto Details(int key)
        {
            GiroDto dto = base.Details(key);

            var allevamenti = this.allevamentiRepository.DbSet.ToList();
            var allevamentiGiro = (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro.Where(a => a.IdGiro == key).ToList();

            foreach (var allevamento in allevamenti)
            {
                var allevamentoXGiro = allevamentiGiro.FirstOrDefault(ag => ag.IdAllevamento == allevamento.Id);
                var utente = allevamento.Utente;

                dto.Items.Add(new GiroItemDto()
                {
                    IdGiro = key,
                    IdAllevamento = allevamento.Id,
                    Allevatore = String.Format("{0} {1}", utente?.Cognome, utente?.Nome),
                    Indirizzo = allevamento.IndirizzoAllevamento,
                    RagioneSociale = utente?.RagioneSociale,
                    Priorita = allevamentoXGiro != null && allevamentoXGiro.Priorita.HasValue ? allevamentoXGiro.Priorita.Value : (int?)null
                });

            }

            dto.Items = dto.Items.OrderByDescending(i => i.Priorita.HasValue).ThenBy(i => i.Priorita).ToList();

            return dto;
        }

        public DropDownDto DropDown(int? idUtente = null)
        {
            var dropdown = new DropDownDto();

            var trasportatoriDropdown = this.trasportatoriService.DropDown(idUtente);
            var trasportatoriIds = trasportatoriDropdown.Items.Select(i => Convert.ToInt32(i.Value)).ToList();

            dropdown.Items = this.repository.DbSet
                .Where(g => trasportatoriIds.Contains(g.IdTrasportatore))
                .OrderBy(i => i.CodiceGiro)
                .ToList()
                .Select(c => new DropDownItem()
                {
                    Value = c.CodiceGiro,
                    Text = $"{c.CodiceGiro} - {c.Denominazione}"
                })
                .ToList();

            return dropdown;
        }

        /// <summary>
        /// Caricamento giri singolo trasportatore
        /// </summary>
        /// <param name="idTrasportatore"></param>
        /// <returns></returns>
        public DropDownDto DropDownByTrasportatore(int idTrasportatore)
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.giriRepository.Query.Where(g => g.IdTrasportatore == idTrasportatore)
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Denominazione
                })
                .ToList();

            return dropdown;
        }

        /// <summary>
        /// Aggiornamento allevatori selezionati e relative priorità
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override GiroDto Update(GiroDto model)
        {            
            List<AllevamentoXGiro> allevamentiDb = (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro.Where(a => a.IdGiro == model.Id).ToList();

            List<AllevamentoXGiro> allevamentiDaEliminare = new List<AllevamentoXGiro>();

            foreach(GiroItemDto item in model.Items)
            {
                AllevamentoXGiro allevamentoDb = allevamentiDb.FirstOrDefault(a => a.IdGiro == model.Id && a.IdAllevamento == item.IdAllevamento);

                // update
                if(allevamentoDb !=  null)
                {
                    if (item.Priorita.HasValue && allevamentoDb.Priorita != item.Priorita)
                        allevamentoDb.Priorita = Convert.ToInt32(item.Priorita);

                    if (!item.Priorita.HasValue)
                        allevamentiDaEliminare.Add(allevamentoDb);
                }


                // insert
                if(allevamentoDb == null && item.Priorita.HasValue)
                {
                    (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro.Add(new AllevamentoXGiro()
                    {
                        IdGiro = model.Id,
                        IdAllevamento = item.IdAllevamento,
                        Priorita = Convert.ToInt32(item.Priorita)
                    });
                }
            }

            // remove
            foreach(AllevamentoXGiro allevamentoDb in allevamentiDb)
            {
                GiroItemDto item = model.Items.FirstOrDefault(i => i.IdAllevamento == allevamentoDb.IdAllevamento);

                if (item == null)
                    allevamentiDaEliminare.Add(allevamentoDb);
            }
            (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro.RemoveRange(allevamentiDaEliminare);

            this.uow.SaveChanges();

            base.Update(model);

            return Details(model.Id);
        }

        protected override Giro UpdateProperties(Giro viewEntity, Giro dbEntity)
        {
            dbEntity.Denominazione = viewEntity.Denominazione;
            dbEntity.CodiceGiro = viewEntity.CodiceGiro;
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;

            return dbEntity;
        }

        #endregion

    }

}

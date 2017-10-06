using System;

using LatteMarche.Application.Giri.Dtos;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.EntityFramework;
using LatteMarche.Application.Allevatori.Dtos;
using System.Data.Entity;

namespace LatteMarche.Application.Giri.Services
{

    public class GiriService: EntityService<Giro, int, GiroDto>, IGiriService
    {

        #region Fields

        private IRepository<Giro, int> giriRepository;
        private IAllevatoriService allevatoriService;      

        #endregion

        #region Constructor

        public GiriService(IUnitOfWork uow, IAllevatoriService allevatoriService)
            : base(uow)
        {
            this.giriRepository = this.uow.Get<Giro, int>();
            this.allevatoriService = allevatoriService;
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

            List<AllevatoreDto> allevatori = this.allevatoriService.Index().ToList();
            List<AllevamentoXGiro> allevamentiGiro = this.uow.Context.AllevamentiXGiro.Where(a => a.IdGiro == key).ToList();

            foreach (AllevatoreDto allevatore in allevatori)
            {
                AllevamentoXGiro allevamentoXGiro = allevamentiGiro.FirstOrDefault(ag => ag.IdAllevamento == allevatore.Id);

                dto.Items.Add(new GiroItemDto()
                {
                    IdGiro = key,
                    IdAllevamento = allevatore.Id,
                    Allevatore = String.Format("{0} {1}", allevatore.Cognome, allevatore.Nome),
                    Indirizzo = allevatore.IndirizzoAllevamento,
                    RagioneSociale = allevatore.RagioneSociale,
                    Priorita = allevamentoXGiro != null ? allevamentoXGiro.Priorita.Value : (int?)null
                });
            }

            return dto;
        }

        /// <summary>
        /// Caricamento giri singolo trasportatore
        /// </summary>
        /// <param name="idTrasportatore"></param>
        /// <returns></returns>
        public List<GiroDto> GetGiriOfTrasportatore(int idTrasportatore)
        {
            return ConvertToDtoList(this.giriRepository.FilterBy(g => g.IdTrasportatore == idTrasportatore).ToList());
        }

        public override GiroDto Update(GiroDto model)
        {            
            List<AllevamentoXGiro> allevamentiDb = this.uow.Context.AllevamentiXGiro.Where(a => a.IdGiro == model.Id).ToList();

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
                    this.uow.Context.AllevamentiXGiro.Add(new AllevamentoXGiro()
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
            this.uow.Context.AllevamentiXGiro.RemoveRange(allevamentiDaEliminare);

            this.uow.SaveChanges();

            return base.Update(model);
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

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

        public override GiroDto Details(int key)
        {
            GiroDto dto = base.Details(key);

            List<AllevatoreDto> allevatori = this.allevatoriService.Index();
            List<AllevamentoXGiro> allevamentiGiro = (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro.Where(a => a.IdGiro == key).ToList();

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
                    Priorita = allevamentoXGiro != null ? allevamentoXGiro.Priorita : (int?)null
                });
            }

            return dto;
        }

        public List<GiroDto> GetGiriOfTrasportatore(int idTrasportatore)
        {
            return ConvertToDtoList(this.giriRepository.FilterBy(g => g.IdTrasportatore == idTrasportatore).ToList());
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

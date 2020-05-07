using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LatteMarche.Application.Latte.Dtos;
using LatteMarche.Application.Latte.Interfaces;
using WeCode.Data.Interfaces;
using WeCode.Application;

namespace LatteMarche.Application.Latte.Services
{
    public class LottiService : EntityService<Lotto, long, LottoDto>, ILottiService
    {

        #region Fields

        private IAllevamentiService allevamentiService;

        #endregion

        #region Constructor

        public LottiService(IUnitOfWork uow, IAllevamentiService allevamentiService)
            : base(uow)
        {
            this.allevamentiService = allevamentiService;
        }

        #endregion

        #region Methods

        public LottoDto GetByCodiceLotto(string codiceLotto)
        {
            var lotti = this.repository.Query.Where(l => l.Codice == codiceLotto && !String.IsNullOrEmpty(l.CodiceSitra));

            if (lotti != null && lotti.Count() > 0)
                return ConvertToDto(lotti.First());
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prelievi"></param>
        /// <returns></returns>
        public List<LottoDto> GetLotti(List<PrelievoLatteDto> prelievi)
        {
            // elenco allevamento con flag alta qualità obbligati all'invio SITRA
            var allevamentiDaInviare = this.allevamentiService.GetAllevamentiSitra();

            // recupero idAllevamenti
            var idAllevamenti = allevamentiDaInviare.Select(a => a.Id).ToList();

            // filtro prelievi degli allevamenti da inviare
            var prelieviDaInviare = prelievi.Where(p => p.IdAllevamento.HasValue && idAllevamenti.Contains(p.IdAllevamento.Value)).ToList();

            // raggruppamento per lotto
            var lotti = prelieviDaInviare
                .GroupBy(u => u.LottoConsegna)
                .Select(grp => new LottoDto()
                {
                    Codice = grp.Key,
                    Quantita = grp.Count(s => s.Quantita.HasValue) == 0 ? 0 : grp.Where(s => s.Quantita.HasValue).Sum(s => s.Quantita.Value),
                    DataConsegna = grp.Max(s => s.DataConsegna) != null ? grp.Max(s => s.DataConsegna).Value : DateTime.Now,
                    DataUltimaMungitura = grp.Max(s => s.DataUltimaMungitura) != null ? grp.Max(s => s.DataUltimaMungitura).Value : DateTime.Now.AddHours(-2),
                    PrelieviPadre = grp.Select(p => Mapper.Map<PrelievoLatteDto>(p)).ToList()
                })
                .ToList();

            return lotti;
        }


        protected override Lotto UpdateProperties(Lotto viewEntity, Lotto dbEntity)
        {
            return dbEntity;
        }

        #endregion

    }
}

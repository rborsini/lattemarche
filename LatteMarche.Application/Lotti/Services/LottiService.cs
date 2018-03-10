using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Application.Allevatori.Interfaces;
using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Application.TipiLatte.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Lotti.Services
{
    public class LottiService : EntityService<Lotto, long, LottoDto>, ILottiService
    {

        //private IAllevatoriService allevatoriService;
        //private ITipiLatteService tipiLatteService;
        //private IAllevamentiService allevamentiService;

        private IRepository<V_Allevamento, int> allevamentiRepository;

        public LottiService(IUnitOfWork uow)
            : base(uow)
        {
            this.allevamentiRepository = uow.Get<V_Allevamento, int>();
        }

        public List<LottoDto> GetLotti(List<PrelievoLatte> prelievi)
        {
            var allevamentiDaInviare = allevamentiRepository.FilterBy(a => a.FlagInvioSitra).ToList();
            var idAllevamenti = allevamentiDaInviare.Select(a => a.Id).ToList();
            //var tipiLatteDaInviare = this.tipiLatteService.Index().Where(t => t.FlagInvioSitra).Select(t => t.Id).ToList();
            //var allevatoriDaInviare = this.allevatoriService.Index().Where(a => a.IdTipoLatte.HasValue && tipiLatteDaInviare.Contains(a.IdTipoLatte.Value)).Select(a => a.Id).ToList();
            //var allevamentiDaInviare = this.allevamentiService.Index().Where(a => allevatoriDaInviare.Contains(a.IdUtente)).Select(a => a.Id).ToList();
            var prelieviDaInviare = prelievi.Where(p => p.IdAllevamento.HasValue && idAllevamenti.Contains(p.IdAllevamento.Value)).ToList();

            foreach(var prelievo in prelieviDaInviare)
            {
                var fattoreConversione = allevamentiDaInviare.First(a => a.Id == prelievo.IdAllevamento).FattoreConversione;
            }

            var lotti = prelieviDaInviare
                .GroupBy(u => u.LottoConsegna)
                .Select(grp => new LottoDto()
                {
                    Codice = grp.Key,
                    Quantita = grp.Count(s => s.Quantita.HasValue) == 0 ? 0 : grp.Where(s => s.Quantita.HasValue).Sum(s => s.Quantita.Value),
                    DataConsegna = grp.Max(s => s.DataConsegna) != null ? grp.Max(s => s.DataConsegna).Value : DateTime.Now,
                    DataUltimaMungitura = grp.Max(s => s.DataUltimaMungitura) != null ? grp.Max(s => s.DataUltimaMungitura).Value : DateTime.Now.AddHours(-2),
                })
                .ToList();
            return lotti;
        }


        protected override Lotto UpdateProperties(Lotto viewEntity, Lotto dbEntity)
        {
            return dbEntity;
        }
    }
}

using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.Lotti.Interfaces;
using LatteMarche.Application.PrelieviLatte.Dtos;
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
        public LottiService(IUnitOfWork uow)
            : base(uow)
        { }

        public List<LottoDto> GetLotti(List<PrelievoLatte> prelievi)
        {
            var lotti = prelievi
                .GroupBy(u => u.LottoConsegna)
                .Select(grp => new LottoDto()
                {
                    Codice = grp.Key,
                    Quantita = grp.Count(s => s.Quantita.HasValue) == 0 ? 0 : grp.Where(s => s.Quantita.HasValue).Sum(s => s.Quantita.Value),
                    TimeStamp = grp.Max().DataConsegna != null ? grp.Max().DataConsegna.Value : DateTime.Now, //TODO: è meglio una data di Default
                    DataUltimaMungitura = grp.Max().DataUltimaMungitura != null ? grp.Max().DataUltimaMungitura.Value : DateTime.Now.AddHours(-2), //TODO: idem come sopra
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

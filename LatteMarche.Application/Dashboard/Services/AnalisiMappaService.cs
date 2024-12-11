using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Filters;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Dashboard.Services
{
    internal class AnalisiMappaService : IAnalisiMappaService
    {
        #region Fields

        private IUnitOfWork uow;

        #endregion

        #region Constructors

        public AnalisiMappaService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        #endregion

        #region Methods

        public WidgetMapDto Load(MapSearchDto searchDto)
        {
            var widget = new WidgetMapDto();

            var acquirentiPalette = new List<string>() { "#ECF8F9", "#068DA9", "#7E1717", "#E55807", "#F6F078", "#01D28E", "#434982", "#730068" };
            var tipiLattePalette = new List<string>() { "#2caffe", "#544fc5", "#00e272", "#fe6a35", "#6b8abc", "#d568fb", "#2ee0ca", "#fa4b42", "#feb56a", "#E5D932" };

            var acquirenti = this.uow.Get<Acquirente, int>().Query.OrderBy(a => a.Id).ToList();
            var tipiLatte = this.uow.Get<TipoLatte, int>().Query.OrderBy(t => t.Id).ToList();

            for (var i = 0; i < acquirenti.Count; i++)
                widget.AcquirentiLegend.Add(new ColorLegend(acquirenti[i].RagioneSociale, acquirentiPalette[i]));

            for (var i = 0; i < tipiLatte.Count; i++)
                widget.TipiLatteLegend.Add(new ColorLegend(tipiLatte[i].Descrizione, tipiLattePalette[i]));

            var query =
            @"
                select

                    allevamento.ID_ALLEVAMENTO as Allevamento_Id,
	                utenti.RAGIONE_SOCIALE as Allevamento,
	                ultimiPrelievi.TipoLatte_Id as TipoLatte_Id,
	                tipiLatte.DESCRIZIONE as TipoLatte,
	                ultimiPrelievi.Prelievo_Id as UltimoPrelievo_Id,
	                PRELIEVO_LATTE.LATITUDINE as Lat,
	                PRELIEVO_LATTE.LONGITUDINE as Lng,
	                PRELIEVO_LATTE.ID_ACQUIRENTE as Acquirente_Id,
	                acquirenti.RAG_SOC_ACQUIRENTE as Acquirente
                from

                    ANAGRAFE_ALLEVAMENTO as allevamento

                    left outer join UTENTI as utenti on allevamento.ID_UTENTE = utenti.ID_UTENTE

                    left outer join
                    (
                        select

                            ID_ALLEVAMENTO as Allevamento_Id,
                            ID_TIPO_LATTE as TipoLatte_Id,
                            max(id_prelievo) as Prelievo_Id

                        from PRELIEVO_LATTE as prelievi
                        group by ID_ALLEVAMENTO, ID_TIPO_LATTE
                    ) as ultimiPrelievi on allevamento.ID_ALLEVAMENTO = UltimiPrelievi.Allevamento_Id


                    left outer join TIPO_LATTE as tipiLatte on tipiLatte.ID_TIPO_LATTE = ultimiPrelievi.TipoLatte_Id


                    left outer join PRELIEVO_LATTE on ultimiPrelievi.Prelievo_Id = PRELIEVO_LATTE.ID_PRELIEVO


                    left outer join ANAGRAFE_ACQUIRENTE as acquirenti on acquirenti.ID_ACQUIRENTE = PRELIEVO_LATTE.ID_ACQUIRENTE


                where

                    ultimiPrelievi.Prelievo_Id is not null
                    and
                    PRELIEVO_LATTE.LATITUDINE is not null
                    and
                    PRELIEVO_LATTE.LONGITUDINE is not null
                    and
                    acquirenti.RAG_SOC_ACQUIRENTE is not null
                    and
                    tipiLatte.DESCRIZIONE is not null
            ";


            if (searchDto.IdAcquirente.HasValue && searchDto.IdAcquirente != 0)
                query += $" AND PRELIEVO_LATTE.ID_ACQUIRENTE =  {searchDto.IdAcquirente} ";

            if (searchDto.IdTipoLatte.HasValue && searchDto.IdTipoLatte != 0)
                query += $" AND ultimiPrelievi.TipoLatte_Id =  {searchDto.IdTipoLatte} ";

            widget.Markers = this.uow.Context.Database.SqlQuery<Marker>(query).ToList();

            foreach(var marker in widget.Markers )
            {
                marker.Acquirente_Color = widget.AcquirentiLegend.First(a => a.Label == marker.Acquirente).Color;
                marker.TipoLatte_Color = widget.TipiLatteLegend.First(a => a.Label == marker.TipoLatte).Color;
            }

            return widget;
        }

        #endregion

    }
}

using LatteMarche.Application.Acquirenti.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Dashboard.Dtos;
using LatteMarche.Application.Dashboard.Filters;
using LatteMarche.Application.Dashboard.Interfaces;
using LatteMarche.Application.Giri.Interfaces;
using LatteMarche.Application.PrelieviLatte.Interfaces;
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

        private IAcquirentiService acquirentiService;
        private ITipiLatteService tipiLatteService;
        private IGiriService giriService;

        #endregion

        #region Constructors

        public AnalisiMappaService(
            IUnitOfWork uow,
            IAcquirentiService acquirentiService,
            ITipiLatteService tipiLatteService,
            IGiriService giriService)
        {
            this.uow = uow;
            this.acquirentiService = acquirentiService;
            this.tipiLatteService = tipiLatteService;
            this.giriService = giriService;
        }

        #endregion

        #region Methods

        public WidgetMapDto Load(MapSearchDto searchDto)
        {
            var widget = new WidgetMapDto();

            var palette = new List<string>() { "#2caffe", "#544fc5", "#00e272", "#fe6a35", "#6b8abc", "#d568fb", "#2ee0ca", "#fa4b42", "#feb56a", "#E5D932" };

            var dropdown = new DropDownDto();

            switch(searchDto.AggregazioneColore)
            {
                case "acquirente":
                    dropdown = this.acquirentiService.DropDown();
                    break;
                case "tipoLatte":
                    dropdown = this.tipiLatteService.DropDown();
                    break;
                case "giro":
                    dropdown = this.giriService.DropDown();
                    break;
            }

            for (var i = 0; i < dropdown.Items.Count; i++)
                widget.Legend.Add(new ColorLegend(dropdown.Items[i].Text, palette[i % palette.Count]));

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
	                acquirenti.RAG_SOC_ACQUIRENTE as Acquirente,
                    giri.CODICE_GIRO            as CodiceGiro,
                    giri.CODICE_GIRO + ' - ' + giri.DENOMINAZIONE          as Giro
                from

                    ANAGRAFE_ALLEVAMENTO as allevamento

                    left outer join UTENTI as utenti on allevamento.ID_UTENTE = utenti.ID_UTENTE

                    inner join
                    (
                        select

                            ID_ALLEVAMENTO      as Allevamento_Id,
                            ID_TIPO_LATTE       as TipoLatte_Id,
                            max(id_prelievo)    as Prelievo_Id
                        from
                            PRELIEVO_LATTE as prelievi
                            left outer join GIRO as giri on prelievi.ID_GIRO = giri.ID_GIRO
                        where 1=1 {prelieviClause}
                        group by ID_ALLEVAMENTO, ID_TIPO_LATTE
                    ) as ultimiPrelievi on allevamento.ID_ALLEVAMENTO = UltimiPrelievi.Allevamento_Id


                    left outer join TIPO_LATTE as tipiLatte on tipiLatte.ID_TIPO_LATTE = ultimiPrelievi.TipoLatte_Id

                    left outer join PRELIEVO_LATTE on ultimiPrelievi.Prelievo_Id = PRELIEVO_LATTE.ID_PRELIEVO

                    left outer join ANAGRAFE_ACQUIRENTE as acquirenti on acquirenti.ID_ACQUIRENTE = PRELIEVO_LATTE.ID_ACQUIRENTE

                    left outer join GIRO as giri on giri.ID_GIRO = PRELIEVO_LATTE.ID_GIRO


                where

                    ultimiPrelievi.Prelievo_Id is not null
                    and PRELIEVO_LATTE.LATITUDINE is not null
                    and PRELIEVO_LATTE.LONGITUDINE is not null
                    and acquirenti.RAG_SOC_ACQUIRENTE is not null
                    and tipiLatte.DESCRIZIONE is not null
                    and giri.DENOMINAZIONE is not null
            ";


            var prelieviClause = "";

            // Periodo Prelievo
            if (searchDto.DataInizio.HasValue || searchDto.DataFine.HasValue)
            {
                DateTime from = searchDto.DataInizio.HasValue ? searchDto.DataInizio.Value : DateTime.MinValue;
                DateTime to = searchDto.DataFine.HasValue ? searchDto.DataFine.Value.AddDays(1) : DateTime.MaxValue;

                prelieviClause += $" AND '{searchDto.DataInizio:yyyy-MM-dd}' <= DATA_PRELIEVO AND DATA_PRELIEVO <= '{searchDto.DataFine:yyyy-MM-dd}' ";
            }

            if (!String.IsNullOrEmpty(searchDto.CodiceGiro))
                prelieviClause += $" AND giri.CODICE_GIRO =  '{searchDto.CodiceGiro}' ";

            if (searchDto.IdTrasportatore.HasValue && searchDto.IdTrasportatore != 0)
                prelieviClause += $" AND ID_TRASPORTATORE =  {searchDto.IdTrasportatore} ";

            if (searchDto.IdAcquirente.HasValue && searchDto.IdAcquirente != 0)
                prelieviClause += $" AND ID_ACQUIRENTE =  {searchDto.IdAcquirente} ";

            if (searchDto.IdTipoLatte.HasValue && searchDto.IdTipoLatte != 0)
                prelieviClause += $" AND ID_TIPO_LATTE =  {searchDto.IdTipoLatte} ";

            query = query.Replace("{prelieviClause}", prelieviClause);

            widget.Markers = this.uow.Context.Database.SqlQuery<Marker>(query).ToList();

            foreach(var marker in widget.Markers )
            {
                var label = "";
                switch(searchDto.AggregazioneColore)
                {
                    case "acquirente":
                        label = marker.Acquirente;
                        break;
                    case "giro":
                        label = marker.Giro;
                        break;
                    case "tipoLatte":
                        label = marker.TipoLatte;
                        break;
                }

                
                marker.Color = widget.Legend.First(a => a.Label == label).Color;
            }

            return widget;
        }

        #endregion

    }
}

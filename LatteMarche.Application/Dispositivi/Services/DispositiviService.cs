using LatteMarche.Application.Dispositivi.Dtos;
using LatteMarche.Application.Dispositivi.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Dispositivi.Services
{
    public class DispositiviService : EntityService<DispositivoMobile, string, DispositivoMobileDto>, IDispositiviService
    {

        #region Fields

        #endregion

        #region Constructor

        public DispositiviService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        { }

        #endregion

        #region Methods

        public override List<DispositivoMobileDto> Index()
        {
            var entities = this.repository.Query.ToList();
            return this.mapper.Map<List<DispositivoMobileDto>>(entities);
        }

        public PagedResult<DispositivoMobileDto> Search(DispositiviSearchDto searchDto)
        {
            IQueryable<DispositivoMobile> query = this.repository.Query;

            // full text
            if (!String.IsNullOrEmpty(searchDto.FullText))
            {
                query = query.Where(u => u.Id.Contains(searchDto.FullText) ||
                                            u.Nome.Contains(searchDto.FullText) ||
                                            u.Marca.Contains(searchDto.FullText) ||
                                            u.Modello.Contains(searchDto.FullText) ||
                                            u.VersioneOS.Contains(searchDto.FullText) ||
                                            u.VersioneApp.Contains(searchDto.FullText) ||
                                            u.Trasportatore.RagioneSociale.Contains(searchDto.FullText) ||
                                            u.Autocisterna.Targa.Contains(searchDto.FullText)
                                    );
            }

            // ordinamento
            query = this.ApplySorting(query, searchDto.Order);

            // paginazione
            var pagedQuery = this.ApplyPaging<DispositivoMobile>(query, searchDto.Start, searchDto.Length);

            var list = pagedQuery.ToList();

            // result dto
            return new PagedResult<DispositivoMobileDto>()
            {
                FilteredList = this.mapper.Map<List<DispositivoMobileDto>>(list),
                Total = query.Count()
            };
        }

        protected override DispositivoMobile UpdateProperties(DispositivoMobile viewEntity, DispositivoMobile dbEntity)
        {
            dbEntity.Attivo = viewEntity.Attivo;
            dbEntity.Nome = viewEntity.Nome;
            dbEntity.IdTrasportatore = viewEntity.IdTrasportatore;
            dbEntity.IdAutocisterna = viewEntity.IdAutocisterna;

            return dbEntity;
        }

        #endregion

    }
}

using System;

using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;



namespace LatteMarche.Application.Comuni.Services
{

    public class ComuniService : EntityReadOnlyService<Comune, int, ComuneDto>, IComuniService
    {

        #region Fields

        private IRepository<Comune, int> comuniRepository;

        #endregion

        #region Constructors

        public ComuniService(IUnitOfWork uow)
            : base(uow)
        {
            this.comuniRepository = this.uow.Get<Comune, int>();
        }

        #endregion

        #region Methods

        public List<ComuneDto> Search(ComuniSearchDto searchDto)
        {
            IQueryable<Comune> query = this.comuniRepository.GetAll();

            // Sigla Provincia
            if(!String.IsNullOrEmpty(searchDto.SiglaProvincia))
            {
                query = query.Where(c => c.Provincia == searchDto.SiglaProvincia);
            }

            return ConvertToDtoList(query.OrderBy(c => c.Descrizione).ToList());
        }

        public List<string> GetProvince()
        {
            return this.repository
                .GetAll()
                .Where(p => !String.IsNullOrEmpty(p.Provincia))
                .Select(p => p.Provincia)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }
    
        #endregion
    }

}

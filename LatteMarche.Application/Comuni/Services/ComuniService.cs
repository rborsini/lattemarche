using System;

using LatteMarche.Application.Comuni.Dtos;
using LatteMarche.Application.Comuni.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;
using LatteMarche.Application.Common.Dtos;

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

        /// <summary>
        /// Ricerca comuni
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public List<ComuneDto> Search(ComuniSearchDto searchDto)
        {
            IQueryable<Comune> query = this.comuniRepository.Query;

            // Sigla Provincia
            if(!String.IsNullOrEmpty(searchDto.SiglaProvincia))
            {
                query = query.Where(c => c.Provincia == searchDto.SiglaProvincia);
            }

            return ConvertToDtoList(query.OrderBy(c => c.Descrizione).ToList());
        }

        /// <summary>
        /// Elenco province
        /// </summary>
        /// <returns></returns>
        public List<string> GetProvince()
        {
            return this.repository
                .Query
                .Where(p => !String.IsNullOrEmpty(p.Provincia))
                .Select(p => p.Provincia)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        public DropDownDto DropDown(string siglaProvincia)
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.repository.DbSet
                .Where(c => c.Provincia == siglaProvincia)
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Descrizione
                })
                .ToList();

            return dropdown;
        }

        #endregion
    }

}

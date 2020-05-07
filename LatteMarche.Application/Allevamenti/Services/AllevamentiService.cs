using System;
using System.Collections.Generic;
using LatteMarche.Application.Allevamenti.Dtos;
using LatteMarche.Application.Allevamenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using LatteMarche.Application.Comuni.Interfaces;
using AutoMapper;
using WeCode.Data.Interfaces;
using WeCode.Application;

namespace LatteMarche.Application.Allevamenti.Services
{

    public class AllevamentiService : EntityService<Allevamento, int, AllevamentoDto>, IAllevamentiService
    {

        #region Fields

        private IRepository<Utente, int> utentiRepository;

        private IComuniService comuniService;

        #endregion

        #region Constructors

        public AllevamentiService(IUnitOfWork uow, IComuniService comuniService)
            : base(uow)
        {
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.comuniService = comuniService;
        }

        #endregion

        #region Methods

        public List<AllevamentoDto> GetAllevamentiSitra()
        {
            return this.repository.Query
                .Where(a => !String.IsNullOrEmpty(a.CUAA))
                .ProjectToList<AllevamentoDto>();
        }


        public List<AllevamentoRowDto> Search(AllevamentiSearchDto searchDto)
        {
            if (searchDto == null)
                searchDto = new AllevamentiSearchDto();

            var query = this.repository.Query;

            // Codice Allevatore
            if (!String.IsNullOrEmpty(searchDto.CodiceAllevatore))
                query = query.Where(a => a.Utente != null && a.Utente.CodiceAllevatore == searchDto.CodiceAllevatore);

            // Codice ASL
            if (!String.IsNullOrEmpty(searchDto.CodiceAsl))
                query = query.Where(a => a.CodiceAsl == searchDto.CodiceAsl);

            var list = query.ToList();

            return query.ProjectToList<AllevamentoRowDto>();
        }

        public override AllevamentoDto Details(int key)
        {
            var dto = base.Details(key);

            if (dto != null && dto.IdComune != 0)
            {
                var comuneObj = this.comuniService.Details(dto.IdComune);

                if (comuneObj != null)
                    dto.SiglaProvincia = comuneObj.Provincia;
            }

            return dto;
        }

        public override AllevamentoDto Update(AllevamentoDto model)
        {
            // Update utente
            var utente = this.utentiRepository.GetById(model.IdUtente.Value);

            if(utente.IdTipoLatte != model.Utente_IdTipoLatte)
            {
                utente.IdTipoLatte = model.Utente_IdTipoLatte.Value;
                this.utentiRepository.Update(utente);
                this.uow.SaveChanges();
            }

            base.Update(model);

            return this.Details(model.Id);
        }

        protected override Allevamento UpdateProperties(Allevamento viewEntity, Allevamento dbEntity)
        {
            dbEntity.CodiceAsl = viewEntity.CodiceAsl;
            dbEntity.IndirizzoAllevamento = viewEntity.IndirizzoAllevamento;
            dbEntity.IdComune = viewEntity.IdComune;
            dbEntity.CUAA = viewEntity.CUAA;

            dbEntity.Latitudine = viewEntity.Latitudine;
            dbEntity.Longitudine = viewEntity.Longitudine;

            return dbEntity;
        }

        #endregion
    }

}

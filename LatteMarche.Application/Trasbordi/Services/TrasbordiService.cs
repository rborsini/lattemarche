using AutoMapper;
using LatteMarche.Application.Trasbordi.Dtos;
using LatteMarche.Application.Trasbordi.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Trasbordi.Services
{
    public class TrasbordiService : ITrasbordiService
    {
        private IUnitOfWork uow;
        private IMapper mapper;

        private IRepository<Trasbordo, long> trasbordiRepository;
        private IRepository<Giro, int> giriRepository;

        public TrasbordiService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.trasbordiRepository = this.uow.Get<Trasbordo, long>();
            this.giriRepository = this.uow.Get<Giro, int>();
        }

        public TrasbordoDto Details(long id)
        {
            var trasbordo = this.trasbordiRepository.GetById(id);

            if (trasbordo == null)
                return null;

            var trasbordoDto = this.mapper.Map<TrasbordoDto>(trasbordo);

            var giro = this.giriRepository.GetById(trasbordo.IdTemplateGiro);
            if (giro != null)
                trasbordoDto.DenominazioneGiro = giro.Denominazione;

            return trasbordoDto;
        }

        public List<TrasbordoDto> Search(TrasbordiSearchDto searchDto)
        {
            if (searchDto == null)
                searchDto = new TrasbordiSearchDto();

            var query = this.trasbordiRepository.Query;

            if (searchDto.DataInizio.HasValue)
                query = query.Where(t => searchDto.DataInizio.Value <= t.Data);

            if (searchDto.DataFine.HasValue)
                query = query.Where(t => t.Data <= searchDto.DataFine.Value);

            var records = query.ToList();

            return this.mapper.Map<List<TrasbordoDto>>(records);
        }
    }
}

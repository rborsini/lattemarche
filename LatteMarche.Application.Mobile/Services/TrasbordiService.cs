using AutoMapper;
using LatteMarche.Application.Mobile.Dtos;
using LatteMarche.Application.Mobile.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Mobile.Services
{
    public class TrasbordiService : ITrasbordiService
    {
        #region Fields

        private IMapper mapper;
        private IUnitOfWork uow;

        private IRepository<Trasbordo, long> repository;
        private IRepository<DispositivoMobile, string> dispositiviRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;

        #endregion

        #region Constructor

        public TrasbordiService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;

            this.repository = this.uow.Get<Trasbordo, long>();
            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Caricamento trasbordo dall'autocisterna di origine
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public TrasbordoDto Push(TrasbordoDto dto)
        {
            var trasbordo = this.mapper.Map<Trasbordo>(dto);

            // associazione imei di destinazione
            trasbordo.IMEI_Destinazione = GetImeiDestinazione(trasbordo.Targa_Destinazione);

            this.repository.Add(trasbordo);
            this.uow.SaveChanges();

            return this.mapper.Map<TrasbordoDto>(trasbordo);
        }

        /// <summary>
        /// Recupero trasbordi disponibili per l'autocisterna di destinazione
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public List<TrasbordoDto> Pull(string imei)
        {
            var trasbordiEntities = this.repository.Query
                .Where(t => t.IMEI_Destinazione == imei && !t.Chiuso)
                .ToList();

            var trasbordiDtos = this.mapper.Map<List<TrasbordoDto>>(trasbordiEntities);

            return trasbordiDtos;
        }

        /// <summary>
        /// Chiusura trasbordo
        /// </summary>
        /// <param name="id"></param>
        public void Close(long id)
        {
            var trasbordo = this.repository.GetById(id);

            if (trasbordo == null)
                return;

            trasbordo.Chiuso = true;
            this.repository.Update(trasbordo);
            this.uow.SaveChanges();
        }

        private string GetImeiDestinazione(string targa_Destinazione)
        {
            var autocisterna = this.autocisterneRepository.Query.FirstOrDefault(a => a.Targa == targa_Destinazione);
            if (autocisterna == null)
                return "";

            var dispositivo = this.dispositiviRepository.Query.FirstOrDefault(d => d.IdAutocisterna == autocisterna.Id);
            if (dispositivo == null)
                return "";

            return dispositivo.Id;
        }

        #endregion

    }
}

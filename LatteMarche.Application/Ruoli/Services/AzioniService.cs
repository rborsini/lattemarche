using AutoMapper;
using LatteMarche.Application.Ruoli.Dtos;
using LatteMarche.Application.Ruoli.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Services
{
    public class AzioniService : EntityService<Azione, string, AzioneDto>, IAzioniService
    {

        public AzioniService(IUnitOfWork uow)
            : base(uow)
        { }

        public void Synch(List<AzioneDto> azioniReflectionDto)
        {
            List<Azione> azioniReflection = Mapper.Map<List<Azione>>(azioniReflectionDto);
            List<Azione> azioniDb = this.repository.GetAll().ToList();

            foreach (Azione azione in azioniReflection)
            {
                Azione azioneDb = azioniDb.FirstOrDefault(a => a.Id == azione.Id);

                if (azioneDb != null)
                {
                    azioneDb = UpdateProperties(azione, azioneDb);
                }
                else
                {
                    repository.Add(azione);
                }

            }

            uow.SaveChanges();
        }

        protected override Azione UpdateProperties(Azione viewEntity, Azione dbEntity)
        {
            dbEntity.Pagina = viewEntity.Pagina;
            dbEntity.Nome = viewEntity.Nome;

            return dbEntity;
        }
    }
}

using AutoMapper;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Collections.Generic;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Auth.Services
{
    public class AzioniService : EntityService<Azione, string, AzioneDto>, IAzioniService
    {

        public AzioniService(IUnitOfWork uow)
            : base(uow)
        { }

        public void Synch(List<AzioneDto> azioniReflectionDto)
        {
            List<Azione> azioniReflection = Mapper.Map<List<Azione>>(azioniReflectionDto);
            List<Azione> azioniDb = this.repository.Query.ToList();

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

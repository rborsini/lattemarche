using AutoMapper;
using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Auth.Services
{
    public class RuoliService : EntityService<Ruolo, long, RuoloDto>, IRuoliService
    {
        private IRepository<Azione, string> azioniRepository;
        private IRepository<Autorizzazione, long> autorizzazioniRepository;
        private IRepository<RuoloUtente, long> ruoliUtenteRepository;

        public RuoliService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
            this.azioniRepository = uow.Get<Azione, string>();
            this.autorizzazioniRepository = uow.Get<Autorizzazione, long>();
            this.ruoliUtenteRepository = uow.Get<RuoloUtente, long>();
        }

        public override RuoloDto Details(long key)
        {
            RuoloDto dto = new RuoloDto();
            Ruolo ruolo = repository.GetById(key);

            if (ruolo != null)
            {
                dto = ConvertToDto(ruolo);

                List<Azione> azioni = azioniRepository.Query.ToList();
                List<Autorizzazione> autorizzazioni = autorizzazioniRepository.Query.Where(a => a.IdRuolo == key).ToList();

                dto.Pagine_MVC = GetPagine(azioni, autorizzazioni, "MVC");
                dto.Pagine_API = GetPagine(azioni, autorizzazioni, "API");

                List<Utente> utenti = ruolo.UtentiRuolo.Select(ur => ur.UtenteObj).OrderBy(u => u.Id).ToList();
                dto.Utenti = this.mapper.Map<List<UtenteDto>>(utenti);
            }

            return dto;
        }



        private List<RuoloDto.Pagina> GetPagine(List<Azione> azioni, List<Autorizzazione> autorizzazioni, string type)
        {
            List<RuoloDto.Pagina> pagine = new List<RuoloDto.Pagina>();

            foreach (string pagina in azioni.Select(a => a.Pagina).Distinct())
            {
                bool paginaAuth = autorizzazioni.Any(a => a.AzioneObj.Pagina == pagina && String.IsNullOrEmpty(a.AzioneObj.ViewItem));
                RuoloDto.Pagina paginaModel = new RuoloDto.Pagina(paginaAuth, pagina);

                List<Azione> azioniPagina = azioni.Where(a => a.Type == type && a.Pagina == pagina && !String.IsNullOrEmpty(a.ViewItem)).ToList();
                foreach (Azione azione in azioniPagina)
                {
                    bool viewItemAuth = autorizzazioni.Any(a => a.AzioneObj.Type == type && a.AzioneObj.Pagina == pagina && a.AzioneObj.ViewItem == azione.ViewItem);
                    paginaModel.Items.Add(new RuoloDto.ViewItemDto(viewItemAuth, azione.ViewItem, azione.Nome));
                }

                if (paginaModel.Items.Count > 0)
                {
                    pagine.Add(paginaModel);
                }
            }

            return pagine;
        }

        public override RuoloDto Update(RuoloDto model)
        {
            Ruolo ruolo = this.mapper.Map<Ruolo>(model);

            // pulizia vecchie autorizzazioni
            List<Autorizzazione> autorizzazioniToRemove = autorizzazioniRepository.Query.Where(a => a.IdRuolo == model.Id).ToList();
            autorizzazioniRepository.Delete(autorizzazioniToRemove);

            // inserimento nuove autorizzazioni
            List<Autorizzazione> autorizzazioniToAdd = ConvertToAutorizzazioni(model.Id, "MVC", model.Pagine_MVC);
            autorizzazioniToAdd.AddRange(ConvertToAutorizzazioni(model.Id, "API", model.Pagine_API));
            autorizzazioniRepository.Add(autorizzazioniToAdd);
            this.uow.SaveChanges();

            // aggiornamento campi ruolo
            return base.Update(model);
        }

        /// <summary>
        /// Aggiornamento ruolo singolo utente
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        public void UpdateUserRole(int userId, long role)
        {
            var ruoliUtente = this.ruoliUtenteRepository.Query.Where(ru => ru.Username == userId).ToList();

            if(ruoliUtente.Count == 1 && ruoliUtente[0].IdRuolo == role)
                return; // non c'è bisogno di fare niente perché il ruolo non è cambiato

            this.ruoliUtenteRepository.Delete(ruoliUtente);
            this.ruoliUtenteRepository.Add(new RuoloUtente()
            {
                IdRuolo = role,
                Username = userId
            });

            this.uow.SaveChanges();
        }

        private List<Autorizzazione> ConvertToAutorizzazioni(long idRuolo, string type, List<RuoloDto.Pagina> pagine)
        {
            List<Azione> azioni = this.azioniRepository.Query.ToList();
            List<Autorizzazione> autorizzazioni = new List<Autorizzazione>();


            foreach (var pagina in pagine.Where(p => !String.IsNullOrEmpty(p.Title)))
            {
                // full page authorization
                if (pagina.Enabled)
                {
                    autorizzazioni.Add(new Autorizzazione()
                    {
                        Azione = azioni.First(a => a.Type == type && a.Pagina == pagina.Title && String.IsNullOrEmpty(a.ViewItem)).Id,
                        IdRuolo = idRuolo
                    });
                }

                foreach (var item in pagina.Items)
                {
                    // view item authorization
                    if (item.Enabled)
                    {
                        Azione azione = azioni.FirstOrDefault(a => a.Type == type && a.Pagina == pagina.Title && a.ViewItem == item.Title);
                        if (azione != null)
                        {
                            autorizzazioni.Add(new Autorizzazione()
                            {
                                Azione = azione.Id,
                                IdRuolo = idRuolo
                            });
                        }
                    }
                }
            }

            return autorizzazioni;
        }


        protected override Ruolo UpdateProperties(Ruolo viewEntity, Ruolo dbEntity)
        {
            dbEntity.Descrizione = viewEntity.Descrizione;

            return dbEntity;
        }

    }
}

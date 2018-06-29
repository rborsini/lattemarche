using AutoMapper;
using LatteMarche.Application.Ruoli.Dtos;
using LatteMarche.Application.Ruoli.Interfaces;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Application.Ruoli.Services
{
    public class RuoliService : EntityService<Ruolo, long, RuoloDto>, IRuoliService
    {
        IRepository<Azione, string> azioniRepository;
        IRepository<Autorizzazione, long> autorizzazioniRepository;

        public RuoliService(IUnitOfWork uow)
            : base(uow)
        {
            this.azioniRepository = uow.Get<Azione, string>();
            this.autorizzazioniRepository = uow.Get<Autorizzazione, long>();
        }

        public override RuoloDto Details(long key)
        {
            RuoloDto dto = new RuoloDto();
            Ruolo ruolo = repository.GetById(key);

            if (ruolo != null)
            {
                dto = ConvertToDto(ruolo);

                List<Azione> azioni = azioniRepository.GetAll().ToList();
                List<Autorizzazione> autorizzazioni = autorizzazioniRepository.FilterBy(a => a.IdRuolo == key).ToList();

                dto.Pagine_MVC = GetPagine(azioni, autorizzazioni, "MVC");
                dto.Pagine_API = GetPagine(azioni, autorizzazioni, "API");

                List<Utente> utenti = ruolo.UtentiRuolo.Select(ur => ur.UtenteObj).OrderBy(u => u.Id).ToList();
                dto.Utenti = Mapper.Map<List<UtenteDto>>(utenti);
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
            Ruolo ruolo = Mapper.Map<Ruolo>(model);

            // pulizia vecchie autorizzazioni
            List<Autorizzazione> autorizzazioniToRemove = autorizzazioniRepository.FilterBy(a => a.IdRuolo == model.Id).ToList();
            autorizzazioniRepository.Delete(autorizzazioniToRemove);

            // inserimento nuove autorizzazioni
            List<Autorizzazione> autorizzazioniToAdd = ConvertToAutorizzazioni(model.Id, "MVC", model.Pagine_MVC);
            autorizzazioniToAdd.AddRange(ConvertToAutorizzazioni(model.Id, "API", model.Pagine_API));
            autorizzazioniRepository.Add(autorizzazioniToAdd);

            // aggiornamento campi ruolo
            return base.Update(model);
        }

        private List<Autorizzazione> ConvertToAutorizzazioni(long idRuolo, string type, List<RuoloDto.Pagina> pagine)
        {
            List<Azione> azioni = this.azioniRepository.GetAll().ToList();
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

        public void RemoveUserInRole(long idRuolo, int username)
        {
            Ruolo ruolo = repository.GetById(idRuolo);
            RuoloUtente ruoloUtente = ruolo.UtentiRuolo.FirstOrDefault(ur => ur.UtenteObj.Id == username);

            if (ruoloUtente != null)
            {
                this.uow.Get<RuoloUtente, long>().Delete(ruoloUtente);
                this.uow.SaveChanges();
            }
        }
    }
}

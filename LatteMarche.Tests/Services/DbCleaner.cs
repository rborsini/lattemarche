using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Tests.Services
{
    public class DbCleaner
    {
        #region Fields

        private IUnitOfWork uow;

        private IRepository<DispositivoMobile, string> dispositiviRepository;

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Lotto, long> lottiRepository;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Utente, int> utentiRepository;
        private IRepository<Acquirente, int> acquirentiRepository;
        private IRepository<UtenteXAcquirente, int> utenteXAcquirenteRepository;
        private IRepository<Destinatario, int> destinatariRepository;
        private IRepository<UtenteXDestinatario, int> utenteXDestinatarioRepository;
        private IRepository<Cessionario, int> cessionariRepository;
        private IRepository<UtenteXCessionario, int> utenteXCessionarioRepository;

        private IRepository<AziendaTrasportatori, int> aziendeTrasportatoriRepository;
        private IRepository<TrasportatoreXAzienda, int> trasportatoriXAziendaRepository;
        private IRepository<Autocisterna, int> autocisterneRepository;



        #endregion

        #region Constructor

        public DbCleaner(IUnitOfWork uow)
        {
            this.uow = uow;

            this.dispositiviRepository = this.uow.Get<DispositivoMobile, string>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.lottiRepository = this.uow.Get<Lotto, long>();
            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.acquirentiRepository = this.uow.Get<Acquirente, int>();
            this.utenteXAcquirenteRepository = this.uow.Get<UtenteXAcquirente, int>();
            this.destinatariRepository = this.uow.Get<Destinatario, int>();
            this.utenteXDestinatarioRepository = this.uow.Get<UtenteXDestinatario, int>();
            this.cessionariRepository = this.uow.Get<Cessionario, int>();
            this.utenteXCessionarioRepository = this.uow.Get<UtenteXCessionario, int>();

            this.aziendeTrasportatoriRepository = this.uow.Get<AziendaTrasportatori, int>();
            this.trasportatoriXAziendaRepository = this.uow.Get<TrasportatoreXAzienda, int>();
            this.autocisterneRepository = this.uow.Get<Autocisterna, int>();
        }

        #endregion

        #region Methods

        public void CleanUp()
        {
            this.prelieviRepository.CleanUp();
            this.lottiRepository.CleanUp();

            this.dispositiviRepository.CleanUp();

            this.autocisterneRepository.CleanUp();

            this.utenteXDestinatarioRepository.CleanUp();
            this.utenteXCessionarioRepository.CleanUp();
            this.utenteXAcquirenteRepository.CleanUp();
            this.trasportatoriXAziendaRepository.CleanUp();

            this.allevamentiRepository.CleanUp();
            this.utentiRepository.CleanUp();
            this.destinatariRepository.CleanUp();
            this.cessionariRepository.CleanUp();
            this.acquirentiRepository.CleanUp();
            this.aziendeTrasportatoriRepository.CleanUp();

            this.uow.SaveChanges();
        }

        #endregion

    }
}

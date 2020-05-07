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
        }

        #endregion

        #region Methods

        public void CleanUp()
        {
            this.prelieviRepository.CleanUp();
            this.lottiRepository.CleanUp();

            this.dispositiviRepository.CleanUp();
            this.allevamentiRepository.CleanUp();
            this.utentiRepository.CleanUp();

            this.uow.SaveChanges();
        }

        #endregion

    }
}

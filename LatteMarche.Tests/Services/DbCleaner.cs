using LatteMarche.Core;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Tests.Services
{
    public class DbCleaner
    {
        #region Fields

        private IUnitOfWork uow;

        private IRepository<Allevamento, int> allevamentiRepository;
        private IRepository<Utente, int> utentiRepository;

        #endregion

        #region Constructor

        public DbCleaner(IUnitOfWork uow)
        {
            this.uow = uow;

            this.allevamentiRepository = this.uow.Get<Allevamento, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
        }

        #endregion

        #region Methods

        public void CleanUp()
        {
            this.allevamentiRepository.CleanUp();
            this.utentiRepository.CleanUp();

            this.uow.SaveChanges();
        }

        #endregion

    }
}

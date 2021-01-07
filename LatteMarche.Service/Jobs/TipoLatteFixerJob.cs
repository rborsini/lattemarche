using Autofac;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace LatteMarche.Service.Jobs
{
    public class TipoLatteFixerJob : BaseJob
    {
        #region Fields

        private IUnitOfWork uow;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Utente, int> utentiRepository;
        private IRepository<Allevamento, int> allevamentiRepository;

        #endregion

        #region Constructor

        public TipoLatteFixerJob()
            : base() { }

        #endregion

        #region Methods

        public override void Execute()
        {
            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                Initialize(scope);

                // recupero prelievi senza tipo latte
                var prelieviQuery = this.prelieviRepository.DbSet
                    .Where(p => !p.IdTipoLatte.HasValue);

                var count = prelieviQuery.Count();

                Console.WriteLine($"Rimangono {count}");

                if (count > 0)
                {
                    var prelievo = prelieviQuery.FirstOrDefault();

                    var allevamento = this.allevamentiRepository.GetById(prelievo.IdAllevamento.Value);

                    var idUtente = allevamento != null ? allevamento.IdUtente.Value : 65;

                    var utente = this.utentiRepository.GetById(idUtente);

                    this.prelieviRepository
                        .DbSet.Where(p => p.IdAllevamento == prelievo.IdAllevamento.Value)
                        .Update(p => new PrelievoLatte()
                        {
                            IdTipoLatte = utente.IdTipoLatte
                        });

                }

                Console.WriteLine("FATTO");
            }
        }

        private void Initialize(ILifetimeScope scope)
        {
            this.uow = scope.Resolve<IUnitOfWork>();

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.utentiRepository = this.uow.Get<Utente, int>();
            this.allevamentiRepository = this.uow.Get<Allevamento, int>();

        }

        #endregion
    }
}

using Autofac;
using LatteMarche.Core.Models;
using LatteMarche.EntityFramework;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Service.Jobs
{
    public class PrelieviFixerJob : BaseJob
    {
        #region Fields

        private IUnitOfWork uow;
        private IRepository<PrelievoLatte, int> prelieviRepository;
        private IRepository<Giro, int> giriRepository;

        #endregion

        #region Constructor

        public PrelieviFixerJob()
            : base() { }

        #endregion

        #region Methods

        public override void Execute()
        {
            using (ILifetimeScope scope = AutoFacConfig.Container.BeginLifetimeScope())
            {
                Initialize(scope);

                // recupero prelievi 2020 senza campi valorizzati (idTrasportatore, idGiro, lottoConsegna)
                var prelievi = this.prelieviRepository.DbSet
                    .Where(p => p.DataPrelievo > new DateTime(2020, 1, 1))
                    .Where(p => !p.IdGiro.HasValue || String.IsNullOrEmpty(p.LottoConsegna) || !p.IdTrasportatore.HasValue)
                    .ToList();

                // ciclo prelievi
                int i = 1;
                foreach(var prelievo in prelievi)
                {
                    // recupero giri configurati per l'allevamento del prelievo
                    var giriAllevamento = (this.uow.Context as LatteMarcheDbContext).AllevamentiXGiro
                        .Where(a => a.IdAllevamento == prelievo.IdAllevamento)
                        .ToList();

                    // conteggio giri configurati per l'allevamento
                    if(giriAllevamento.Count == 1)
                    {
                        // caricamento giro associato
                        var idGiro = giriAllevamento[0].IdGiro;
                        var giro = this.giriRepository.GetById(idGiro);

                        // idGiro
                        prelievo.IdGiro = idGiro;

                        // idTrasportatore
                        if (!prelievo.IdTrasportatore.HasValue)
                            prelievo.IdTrasportatore = giro.IdTrasportatore;

                        // lottoConsegna
                        if (String.IsNullOrEmpty(prelievo.LottoConsegna))
                            prelievo.LottoConsegna = $"{giro?.CodiceGiro}{prelievo.DataConsegna:ddMMyyHHmm}";

                        // Update
                        this.prelieviRepository.Update(prelievo);
                        this.uow.SaveChanges();

                    }
                    else
                    {
                        // manca solo IdGiro => recupero il giro dal lotto consegna
                        if(!prelievo.IdGiro.HasValue && !String.IsNullOrEmpty(prelievo.LottoConsegna))
                        {
                            var codiceGiro = prelievo.LottoConsegna.Substring(0, 2);
                            var giro = this.giriRepository.DbSet.FirstOrDefault(g => g.CodiceGiro == codiceGiro);

                            if(giro != null)
                            {
                                prelievo.IdGiro = giro.Id;
                                this.prelieviRepository.Update(prelievo);
                                this.uow.SaveChanges();
                            }
                        }

                        // lotto consegna null
                        if(String.IsNullOrEmpty(prelievo.LottoConsegna))
                        {
                            var prelievoBuono = this.prelieviRepository.DbSet.FirstOrDefault(p => p.DataConsegna == prelievo.DataConsegna && !String.IsNullOrEmpty(p.LottoConsegna));

                            if(prelievoBuono != null)
                            {
                                prelievo.IdTrasportatore = prelievoBuono.IdTrasportatore;
                                prelievo.IdGiro = prelievoBuono.IdGiro;
                                prelievo.LottoConsegna = prelievoBuono.LottoConsegna;

                                this.prelieviRepository.Update(prelievo);
                                this.uow.SaveChanges();
                            }

                        }

                        // idtrasportatore non nullo
                        if(prelievo.IdTrasportatore.HasValue)
                        {
                            var giriTrasportatore = this.giriRepository.DbSet.Where(g => g.IdTrasportatore == prelievo.IdTrasportatore).Select(ga => ga.Id).ToList();

                            var giroAllevamento = giriAllevamento.FirstOrDefault(ga => giriTrasportatore.Contains(ga.IdGiro));
                            if(giroAllevamento != null)
                            {
                                var idGiro = giroAllevamento.IdGiro;

                                // idGiro
                                prelievo.IdGiro = idGiro;

                                // lottoConsegna
                                if (String.IsNullOrEmpty(prelievo.LottoConsegna))
                                {
                                    var giro = this.giriRepository.GetById(idGiro);
                                    prelievo.LottoConsegna = $"{giro?.CodiceGiro}{prelievo.DataConsegna:ddMMyyHHmm}";
                                }

                                // Update
                                this.prelieviRepository.Update(prelievo);
                                this.uow.SaveChanges();

                            }

                        }

                    }

                    Console.WriteLine($"{i} di {prelievi.Count}");
                    i++;
                }

                Console.WriteLine("FATTO");
                //Console.ReadKey();
            }
        }

        private void Initialize(ILifetimeScope scope)
        {
            this.uow = scope.Resolve<IUnitOfWork>();

            this.prelieviRepository = this.uow.Get<PrelievoLatte, int>();
            this.giriRepository = this.uow.Get<Giro, int>();

        }

        #endregion
    }
}

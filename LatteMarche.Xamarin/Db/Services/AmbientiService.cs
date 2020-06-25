using LatteMarche.Xamarin.Db.Interfaces;
using LatteMarche.Xamarin.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Xamarin.Db.Services
{
    public class AmbientiService : BaseEntityService<Ambiente, int>, IAmbientiService
    {
        #region Constants

        private const string TEST_URL = "http://robertoborsini.myqnapcloud.com:81";
        private const string PROD_URL = "http://lattemarche.azurewebsites.net/";

        #endregion

        #region Methods

        public List<Ambiente> Init()
        {
            var ambienti = this.GetItemsAsync().Result.ToList();

            if(ambienti.Count == 0)
            {
                ambienti.Add(new Ambiente() { Nome = "Test", Url = TEST_URL, Selezionato = false });
                ambienti.Add(new Ambiente() { Nome = "Prod", Url = PROD_URL, Selezionato = true });

                this.AddRangeItemAsync(ambienti).Wait();
            }

            return ambienti;

        }

        public void SetDefault(int idAmbiente)
        {
            using (var context = CreateContext())
            {
                var ambienti = context.Set<Ambiente>().ToList();

                foreach (var ambiente in ambienti)
                {
                    ambiente.Selezionato = ambiente.Id == idAmbiente;
                    context.Update<Ambiente>(ambiente);
                }

                context.SaveChanges();
            }
        }

        public Ambiente GetDefault()
        {
            using (var context = CreateContext())
            {
                return context.Set<Ambiente>()
                    .FirstOrDefault(a => a.Selezionato);
            }
        }

        #endregion

    }
}

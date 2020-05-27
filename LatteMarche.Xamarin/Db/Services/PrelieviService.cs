using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LatteMarche.Xamarin.Interfaces;
using LatteMarche.Xamarin.Db;
using LatteMarche.Xamarin.Db.Models;
using LatteMarche.Xamarin.Db.Interfaces;

namespace LatteMarche.Xamarin.Db.Services
{
    public class PrelieviService : BaseEntityService<Prelievo, string>, IPrelieviService
    {

        public PrelieviService()
        { }


        public async Task<IEnumerable<Prelievo>> GetByGiro(int idGiro)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Prelievo>()
                    .Where(g => g.IdGiro.HasValue && g.IdGiro.Value == idGiro)
                    .ToListAsync();
            }
        }

        public async override Task<Prelievo> GetItemAsync(string id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<Prelievo>()
                    .Include(l => l.Giro)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id.Equals(id));
            }
        }

        protected override Prelievo UpdateProperties(Prelievo entityItem, Prelievo viewItem)
        {
            entityItem.Titolo = viewItem.Titolo;
            entityItem.DataConsegna = viewItem.DataConsegna;
            entityItem.DataPrelievo = viewItem.DataPrelievo;
            entityItem.DataUltimaMungitura = viewItem.DataUltimaMungitura;
            entityItem.IdAllevamento = viewItem.IdAllevamento;
            entityItem.IdAcquirente = viewItem.IdAcquirente;
            entityItem.IdCessionario = viewItem.IdCessionario;
            entityItem.IdDestinatario = viewItem.IdDestinatario;
            entityItem.NumeroMungiture = viewItem.NumeroMungiture;
            entityItem.Quantita_kg = viewItem.Quantita_kg;
            entityItem.Quantita_lt = viewItem.Quantita_lt;
            entityItem.Scomparto = viewItem.Scomparto;
            entityItem.Temperatura = viewItem.Temperatura;

            return entityItem;
        }
    }
}

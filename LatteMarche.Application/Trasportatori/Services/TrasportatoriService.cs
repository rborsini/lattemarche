using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Trasportatori.Interfaces;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Trasportatori.Services
{
    public class TrasportatoriService : ITrasportatoriService
    {

        private IRepository<Utente, int> utentiRepository;

        public TrasportatoriService(IUnitOfWork uow)
        {
            this.utentiRepository = uow.Get<Utente, int>();
        }

        public DropDownDto DropDown()
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.utentiRepository.DbSet
                .Where(u => u.IdProfilo ==  5)
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.RagioneSociale
                })
                .ToList();

            return dropdown;
        }

    }
}

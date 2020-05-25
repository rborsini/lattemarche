using LatteMarche.Application.Auth.Dtos;
using LatteMarche.Application.Auth.Interfaces;
using LatteMarche.Application.Common.Dtos;
using LatteMarche.Application.Utenti.Dtos;
using LatteMarche.Application.Utenti.Interfaces;
using LatteMarche.Core;
using LatteMarche.Core.Models;
using System.Linq;
using WeCode.Application;
using WeCode.Data.Interfaces;

namespace LatteMarche.Application.Utenti.Services
{

    public class TipiProfiloService: EntityReadOnlyService<TipoProfilo, int, TipoProfiloDto>, ITipiProfiloService
    {


        #region Constructors

        public TipiProfiloService(IUnitOfWork uow)
            : base(uow)
        { }

        #endregion

        #region Methods

        public int GetIdProfilo(string DescrizioneProfilo)
        {
            return this.repository.Query.FirstOrDefault(t => t.Descrizione == DescrizioneProfilo).Id;
        }

        public DropDownDto DropDown()
        {
            var dropdown = new DropDownDto();

            dropdown.Items = this.repository.DbSet
                //.ToList()
                .Select(c => new DropDownItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Descrizione
                })
                .ToList();

            return dropdown;
        }

        #endregion

    }

}

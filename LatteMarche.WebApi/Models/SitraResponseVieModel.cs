using LatteMarche.Application.Lotti.Dtos;
using LatteMarche.Application.PrelieviLatte.Dtos;
using LatteMarche.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatteMarche.WebApi.Models
{
    public class SitraResponseVieModel
    {

        private bool sitraEnabled;

        public bool SitraEnabled
        {
            get { return this.sitraEnabled; }
            set
            {
                this.sitraEnabled = value;
                if(!value)
                {
                    this.PrelieviInviati = null;
                    this.LottiInviati = null;
                }
            }
        }

        public List<PrelievoLatteDto> PrelieviInviati { get; set; }

        public List<LottoDto> LottiInviati { get; set; }

        public SitraResponseVieModel()
        {
            this.PrelieviInviati = new List<PrelievoLatteDto>();
            this.LottiInviati = new List<LottoDto>();
        }

    }
}